(function () {
    'use strict';

    //用户列表组件
    angular.module('app.main')
        .controller('usersListComponentController', [
            '$uibModal', '$q', '$timeout', 'abp.services.portal.user', 'abp.services.portal.userForHost',
            function ($uibModal, $q, $timeout, userService, userForHostService) {

                var vm = this;
                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize,
                    sorting: null
                };

                vm.userLoading = false;

                //用户列表
                vm.userGridOptions = {
                    //菜单
                    enableColumnMenus: false,
                    //选中
                    enableRowSelection: true,
                    enableSelectAll: false,
                    multiSelect: false,
                    //数据
                    paginationPageSizes: App.consts.grid.defaultPageSizes,
                    paginationPageSize: App.consts.grid.defaultPageSize,
                    useExternalPagination: true,
                    useExternalSorting: true,
                    appScopeProvider: vm,
                    data: [],
                    columnDefs: [
                        {
                            name: '用户名',
                            field: 'userName',
                            width: 100
                        }, {
                            name: '姓名',
                            field: 'surname',
                            width: 100
                        }, {
                            name: '邮件地址',
                            field: 'emailAddress'
                        }, {
                            name: '状态',
                            field: 'isActive',
                            width: 65,
                            cellTemplate:
                                '<div class=\"ui-grid-cell-contents text-center\">' +
                                    '  <span ng-show="row.entity.isActive" class="label label-success">' + '启用' + '</span>' +
                                    '  <span ng-show="!row.entity.isActive" class="label label-default">' + '禁用' + '</span>' +
                                '</div>'
                        }, {
                            name: 'rowOperator',
                            displayName: '',
                            enableSorting: false,
                            width: 80,
                            cellTemplate:
                                '<div class=\"ui-grid-cell-contents\">' +
                                    '   <button class=\"btn btn-xs btn-flat bg-silver yellow-stripe\" ng-click="grid.appScope.editUser(row.entity)">' + '<i class=\"fa fa-fw fa-pencil\"></i>编辑' + '</button>' +
                                '</div>'
                        }
                    ],
                    onRegisterApi: function (gridApi) {
                        vm.userGridOptions.gridApi = gridApi;
                        //排序
                        vm.userGridOptions.gridApi.core.on.sortChanged(null, function (grid, sortColumns) {
                            if (!sortColumns.length || !sortColumns[0].field) {
                                requestParams.sorting = null;
                            } else {
                                requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                            }

                            vm.bindUsers();
                        });
                        //分页
                        vm.userGridOptions.gridApi.pagination.on.paginationChanged(null, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindUsers();
                        });
                        //行选中事件
                        vm.userGridOptions.gridApi.selection.on.rowSelectionChanged(null, function (row, event) {
                            vm.selectedUser = row && row.isSelected ? row.entity : null;
                        });
                    }
                };

                //绑定用户
                vm.bindUsers = function () {
                    vm.userLoading = true;
                    //清除选中用户
                    vm.selectedUser = null;

                    var deferred = $q.defer();
                    var servicePromise = null;
                    if (vm.tenantId) {
                        //特定租户用户
                        servicePromise = userForHostService.getUsers({
                            skipCount: requestParams.skipCount,
                            maxResultCount: requestParams.maxResultCount,
                            sorting: requestParams.sorting,
                            filter: vm.filterText,
                            isActive: vm.filterIsActive
                        }, vm.tenantId);
                    } else {
                        //当前租户用户
                        servicePromise = userService.getUsers({
                            skipCount: requestParams.skipCount,
                            maxResultCount: requestParams.maxResultCount,
                            sorting: requestParams.sorting,
                            filter: vm.filterText,
                            isActive: vm.filterIsActive
                        });
                    }

                    //数据返回
                    servicePromise.success(function (data) {
                        vm.userGridOptions.totalItems = data.totalCount;
                        vm.userGridOptions.data = data.items;

                        deferred.resolve(data); //resolve promise
                    }).finally(function () {
                        vm.userLoading = false;
                    });

                    return deferred.promise;
                };

                //分配角色
                vm.assignRole = function () {
                    if (vm.selectedUser) {
                        var selectedUserId = vm.selectedUser.id;
                        var selectedRoleNames = [];
                        if (vm.getSelectedRoles)
                            selectedRoleNames = vm.getSelectedRoles();

                        //分配用户角色      
                        var servicePromise = null;
                        if (vm.tenantId) {
                            //特定租户用户
                            servicePromise = userForHostService.assignRoles({
                                userId: selectedUserId,
                                assignedRoleNames: selectedRoleNames
                            }, vm.tenantId);
                        } else {
                            //当前租户用户
                            servicePromise = userService.assignRoles({
                                userId: selectedUserId,
                                assignedRoleNames: selectedRoleNames
                            });
                        }

                        servicePromise.success(function () {
                            abp.notify.success("角色分配成功！");

                            //重新加载用户数据
                            vm.bindUsers()
                                .then(function (data) {
                                    //设置选中原来的用户
                                    $timeout(function () {
                                        if (vm.userGridOptions.gridApi) {
                                            angular.forEach(vm.userGridOptions.data, function (user) {
                                                if (user.id === selectedUserId) {
                                                    vm.userGridOptions.gridApi.selection.selectRow(user);
                                                }
                                            });
                                        }
                                    });
                                });
                        });

                    } else {
                        abp.notify.warn("请选择一个用户用于分配角色！");
                    }
                };

                //打开创建用户模态框
                vm.createUser = function () {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/users/create.cshtml',
                        controller: 'userCreateController as vm',
                        backdrop: 'static',
                        resolve: {
                            tenantId: function () {
                                return vm.tenantId;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindUsers();
                    });
                };

                //打开编辑用户模态框
                vm.editUser = function (user) {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/users/edit.cshtml',
                        controller: 'userEditController as vm',
                        backdrop: 'static',
                        resolve: {
                            userId: function () {
                                return user.id;
                            },
                            tenantId: function () {
                                return vm.tenantId;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindUsers();
                    });
                };

                //init
                vm.bindUsers();

            } //end controller
        ])
        .directive('usersListComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    tenantId: '@',
                    filterText: '=',
                    filterIsActive: '=',
                    selectedUser: '=',   //当前选中用户
                    getSelectedRoles: '&'
                },
                controller: 'usersListComponentController',
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '~/App/Main/views/users/usersListComponent.cshtml'
            };
        });
})();