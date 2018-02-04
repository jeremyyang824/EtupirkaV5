(function () {
    'use strict';

    angular.module('app.main')
        .controller('rolesListComponentController', [
            '$scope', '$uibModal', '$q', '$timeout', 'abp.services.portal.role', 'abp.services.portal.roleForHost',
            function ($scope, $uibModal, $q, $timeout, roleService, roleForHostService) {

                var vm = this;

                vm.roleLoading = false;
                vm.selectedRoles = [];

                //角色列表
                vm.roleGridOptions = {
                    //菜单
                    enableColumnMenus: false,
                    //选中
                    enableRowSelection: true,
                    enableSelectAll: true,
                    multiSelect: true,
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
                        name: '',
                        field: 'isAssigned',
                        width: 65,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '  <span ng-show="row.entity.isAssigned" class="label label-success">' + '已分配' + '</span>' +
                            '</div>'
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.roleGridOptions.gridApi = gridApi;
                        //行选中事件
                        vm.roleGridOptions.gridApi.selection.on.rowSelectionChanged(null, function (row, event) {
                            //vm.selectedUser = row && row.isSelected ? row.entity : null;
                            vm.selectedRoles = vm.roleGridOptions.gridApi.selection.getSelectedRows();
                        });
                        vm.roleGridOptions.gridApi.selection.on.rowSelectionChangedBatch(null, function (row, event) {
                            vm.selectedRoles = vm.roleGridOptions.gridApi.selection.getSelectedRows();
                        });
                    }
                };

                //绑定角色
                vm.bindRoles = function () {
                    vm.roleLoading = true;

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

                        //绑定选中用户（若存在）的角色
                        vm.bindSelectedUserRoles();

                    }).finally(function () {
                        vm.roleLoading = false;
                    });
                };

                //标识用户角色
                vm.bindSelectedUserRoles = function () {

                    var assignedRoles = vm.assignedRoles;

                    if (vm.roleGridOptions.gridApi)
                        vm.roleGridOptions.gridApi.selection.clearSelectedRows();

                    //遍历所有角色，将当前选中用户的角色标识出来
                    angular.forEach(vm.roleGridOptions.data, function (roleItem) {
                        //遍历应有角色Id
                        roleItem.isAssigned = false;

                        if (assignedRoles) {
                            for (var i in assignedRoles) {
                                if (roleItem.id === assignedRoles[i]) {
                                    roleItem.isAssigned = true;

                                    if (vm.roleGridOptions.gridApi)
                                        vm.roleGridOptions.gridApi.selection.selectRow(roleItem);

                                    break;
                                }
                            }
                        }

                    });
                };

                //监视assignedRoles
                $scope.$watch("vm.assignedRoles",
                    function (newVal, oldVal) {
                        if (newVal === oldVal)
                            return;
                        vm.bindSelectedUserRoles();
                    });

                //init                
                vm.bindRoles();

            } //end controller
        ])
        .directive('rolesListComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    tenantId: '@',
                    assignedRoles: '=',  //已分配角色数组
                    selectedRoles: '='   //选择的角色数组
                },
                controller: 'rolesListComponentController',
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '~/App/Main/views/users/rolesListComponent.cshtml'
            };
        });
})();