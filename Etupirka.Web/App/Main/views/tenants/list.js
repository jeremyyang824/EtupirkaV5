(function () {
    'use strict';

    angular.module('app.main')
        .controller('tenantsListController', [
            '$scope', '$rootScope', '$stateParams', '$uibModal', 'abp.services.portal.tenant',
            function ($scope, $rootScope, $stateParams, $uibModal, tenantService) {

                $rootScope.setViewTitle('烟厂查询');  //设置页标题

                var vm = this;
                vm.loading = false;
                vm.filterText = $stateParams.filterText || null;
                vm.filterIsActive = App.util.parseBool($stateParams.filterIsActive);

                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize,
                    sorting: null
                };

                //烟厂列表
                vm.tenantGridOptions = {
                    //菜单
                    enableColumnMenus: false,
                    //选中
                    //enableRowSelection: true,
                    //enableSelectAll: true,
                    //selectionRowHeaderWidth: 35,
                    //数据
                    paginationPageSizes: App.consts.grid.defaultPageSizes,
                    paginationPageSize: App.consts.grid.defaultPageSize,
                    useExternalPagination: true,
                    useExternalSorting: true,
                    appScopeProvider: vm,
                    data: [],
                    columnDefs: [{
                        name: '烟厂编码',
                        field: 'tenancyName',
                        width: 100
                    }, {
                        name: '烟厂名称',
                        field: 'name'
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
                        name: '创建时间',
                        field: 'creationTime',
                        cellFilter: 'momentFormat: \'L\'',
                        width: 95
                    }, {
                        name: 'rowOperator',
                        displayName: '',
                        enableSorting: false,
                        width: 310,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '   <button class=\"btn btn-xs btn-flat bg-silver\" ng-class=\"{true:\'red-stripe\', false:\'green-stripe\'}[row.entity.isActive]\" ng-click="grid.appScope.toggleTenantState(row.entity)">' +
                            '       <i class=\"fa fa-fw fa-refresh\"></i><span ng-bind=\"(row.entity.isActive ? \'禁用\' : \'启用\')\"></span>' +
                            '   </button>' +
                            '   <button class=\"btn btn-xs btn-flat bg-silver yellow-stripe\" ng-click="grid.appScope.editTenant(row.entity)">' + '<i class=\"fa fa-fw fa-pencil\"></i>编辑' + '</button>' +
                            '   <a class=\"btn btn-xs btn-flat bg-silver blue-stripe\" ui-sref=\"tenants.userlist({tenantId: row.entity.id})\" etu-breadcrumbs-push=\"烟厂用户\">' + '<i class=\"fa fa-fw fa-user\"></i>人员' + '</a>' +
                            '   <a class=\"btn btn-xs btn-flat bg-silver blue-stripe\" ui-sref=\"tenants.rolelist({tenantId: row.entity.id})\" etu-breadcrumbs-push=\"烟厂角色\">' + '<i class=\"fa fa-fw fa-group\"></i>角色' + '</a>' +
                            '   <a class=\"btn btn-xs btn-flat bg-silver blue-stripe\" ui-sref=\"tenants.inventorylist({tenantId: row.entity.id})\" etu-breadcrumbs-push=\"烟厂库存\">' + '<i class=\"fa fa-fw fa-inbox\"></i>库存' + '</a>' +
                            '</div>'
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.tenantGridOptions.gridApi = gridApi;
                        //行首
                        vm.tenantGridOptions.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 35, cellTemplate: 'etu-ui-grid/rowHeader' });
                        //排序
                        vm.tenantGridOptions.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                            if (!sortColumns.length || !sortColumns[0].field) {
                                requestParams.sorting = null;
                            } else {
                                requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                            }

                            vm.bindTenants();
                        });
                        //分页
                        vm.tenantGridOptions.gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindTenants();
                        });
                    }
                };


                //绑定租户
                vm.bindTenants = function () {
                    vm.loading = true;
                    tenantService.getTenants({
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        sorting: requestParams.sorting,
                        filter: vm.filterText,
                        isActive: vm.filterIsActive
                    }).success(function (data) {
                        vm.tenantGridOptions.totalItems = data.totalCount;
                        vm.tenantGridOptions.data = data.items;
                    }).finally(function () {
                        vm.loading = false;
                    });
                };

                //打开创建烟厂模态框
                vm.createTenant = function () {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/tenants/create.cshtml',
                        controller: 'tenantCreateController as vm',
                        backdrop: 'static'
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindTenants();
                    });
                };

                //打开烟厂编辑模态框
                vm.editTenant = function (tenant) {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/tenants/edit.cshtml',
                        controller: 'tenantEditController as vm',
                        backdrop: 'static',
                        resolve: {
                            tenantId: function () {
                                return tenant.id;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindTenants();
                    });
                };

                //改变烟厂可用状态
                vm.toggleTenantState = function (tenant) {
                    if (!tenant) {
                        abp.notify.error('无法获取操作的烟厂', '错误');
                        return;
                    }

                    var isCurrentActive = tenant.isActive;
                    var confirmMsg = abp.utils.formatString('是否{0}[{1} - {2}]?',
                        isCurrentActive ? '禁用' : '启用', tenant.tenancyName, tenant.name);
                    abp.message.confirm(confirmMsg)
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.loading = true;
                            tenantService.toggleTenantState(tenant.id, !isCurrentActive)
                                .success(function () {
                                    vm.bindTenants();
                                })
                                .finally(function () {
                                    vm.loading = false;
                                });
                        });

                };

                vm.bindTenants();

            } //end controller
        ]);
})();