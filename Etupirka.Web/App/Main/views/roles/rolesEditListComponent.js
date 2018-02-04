(function () {
    'use strict';

    angular.module('app.main')
        .controller('rolesEditListComponentController', [
            '$scope', '$uibModal', '$q', '$timeout', 'abp.services.portal.role', 'abp.services.portal.roleForHost',
            function ($scope, $uibModal, $q, $timeout, roleService, roleForHostService) {

                var vm = this;

                vm.roleLoading = false;

                //角色列表
                vm.roleGridOptions = {
                    //菜单
                    enableColumnMenus: false,
                    //选中
                    enableRowSelection: true,
                    enableSelectAll: false,
                    multiSelect: false,
                    //数据
                    appScopeProvider: vm,
                    data: [],
                    columnDefs: [{
                        name: '角色名',
                        field: 'name',
                        width: 160
                    }, {
                        name: '角色描述',
                        field: 'displayName'
                    }, {
                        name: 'rowOperator',
                        displayName: '',
                        enableSorting: false,
                        width: 80,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                                '   <button class=\"btn btn-xs btn-flat bg-silver yellow-stripe\" ng-click="grid.appScope.editRole(row.entity)">' + '<i class=\"fa fa-fw fa-pencil\"></i>编辑' + '</button>' +
                            '</div>'
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.roleGridOptions.gridApi = gridApi;
                        //行选中事件
                        vm.roleGridOptions.gridApi.selection.on.rowSelectionChanged(null, function (row, event) {
                            vm.selectedRole = row && row.isSelected ? row.entity : null;
                            //取得选中角色的权限
                            if (vm.selectedRole && vm.selectedRole.id) {
                                getRolePermissions(vm.selectedRole.id).then(function (data) {
                                    vm.selectedPermissions = data;
                                });
                            }
                        });
                    }
                };

                //绑定角色
                vm.bindRoles = function () {
                    vm.roleLoading = true;
                    vm.selectedPermissions = [];  //清除选中的权限

                    var servicePromise = null;
                    if (vm.tenantId) {
                        //特定租户用户
                        servicePromise = roleForHostService.getRoles(vm.tenantId);
                    } else {
                        //当前租户用户
                        servicePromise = roleService.getRoles();
                    }

                    servicePromise.success(function (data) {
                        vm.roleGridOptions.data = data.items;
                    }).finally(function () {
                        vm.roleLoading = false;
                    });
                };

                //分配角色权限
                vm.assignRolePermissions = function () {
                    if (vm.selectedRole) {
                        var servicePromise = null;
                        if (vm.tenantId) {
                            //特定租户用户
                            servicePromise = roleForHostService.updateRolePermissions({
                                roleId: vm.selectedRole.id,
                                grantedPermissionNames: vm.selectedPermissions
                            }, vm.tenantId);
                        } else {
                            //当前租户用户
                            servicePromise = roleService.updateRolePermissions({
                                roleId: vm.selectedRole.id,
                                grantedPermissionNames: vm.selectedPermissions
                            });
                        }

                        servicePromise.success(function () {
                            abp.notify.success("权限分配成功！");     
                        });

                    } else {
                        abp.notify.warn("请选择一个角色用于分配权限！");
                    }
                };

                //创建角色模态框
                vm.createRole = function() {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/roles/create.cshtml',
                        controller: 'roleCreateController as vm',
                        backdrop: 'static',
                        resolve: {
                            tenantId: function () {
                                return vm.tenantId;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindRoles();
                    });
                };

                //编辑角色模态框
                vm.editRole = function(role) {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/roles/edit.cshtml',
                        controller: 'roleEditController as vm',
                        backdrop: 'static',
                        resolve: {
                            roleId: function () {
                                return role.id;
                            },
                            tenantId: function () {
                                return vm.tenantId;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindRoles();
                    });
                };

                //取得角色权限
                function getRolePermissions(roleId) {
                    var deferred = $q.defer();

                    var servicePromise = null;
                    if (vm.tenantId) {
                        //特定租户用户
                        servicePromise = roleForHostService.getRolePermissions(roleId, vm.tenantId);
                    } else {
                        //当前租户用户
                        servicePromise = roleService.getRolePermissions(roleId);
                    }

                    servicePromise.success(function (data) {
                        deferred.resolve(data);
                    });

                    return deferred.promise;
                };

                //init                
                vm.bindRoles();
            }
        ])
        .directive('rolesEditListComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    tenantId: '@',
                    selectedRole: '=',          //当前选中角色
                    selectedPermissions: '='    //角色被授予的权限
                },
                controller: 'rolesEditListComponentController',
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '~/App/Main/views/roles/rolesEditListComponent.cshtml'
            };
        });

})();