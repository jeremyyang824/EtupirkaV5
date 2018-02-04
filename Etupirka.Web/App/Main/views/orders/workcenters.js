(function () {
    'use strict';

    angular.module('app.main')
        .controller('dispatchedWorkCentersController', [
            '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', 'etuBreadcrumbsService', 'abp.services.manufacture.workCenter', 'abp.services.manufacture.dispatchedPrepare',
            function ($scope, $rootScope, $state, $stateParams, $uibModal, etuBreadcrumbsService, workcenterService, dispatchedPrepareService) {

                $rootScope.setViewTitle('机台作业');  //设置页标题

                var vm = this;
                vm.loading = false;
                vm.filterType = 'stWorkCenterCode';//,{name:'订单号',value:'stOrderNumber'}
                vm.filterText = '';

                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize
                };

                //交接单单列表
                vm.workCenterGridOptions = {
                    //菜单
                    enableColumnMenus: false,
                    enableSorting: false,
                    //数据
                    paginationPageSizes: App.consts.grid.defaultPageSizes,
                    paginationPageSize: App.consts.grid.defaultPageSize,
                    useExternalPagination: true,
                    useExternalSorting: true,
                    appScopeProvider: vm,
                    data: [],
                    columnDefs: [{
                        name: '工作中心编码',
                        field: 'workCenterCode',
                        width: 200
                    }, {
                        name: '工作中心',
                        field: 'workCenterName',
                        width: 200
                    }
                    //, {
                    //    name: '已下达数',
                    //    field: '',
                    //    width: 100
                    //}
                    , {
                        name: 'rowOperator',
                        displayName: '',
                        enableSorting: false,
                        width: 85,
                        pinnedRight: true,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '   <button class=\"btn btn-xs btn-flat bg-silver blue-stripe\" ng-click="grid.appScope.showOrders(row.entity)">' + '<i class=\"fa fa-fw fa-history\"></i>明细' + '</button>' +
                            '</div>'
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.workCenterGridOptions.gridApi = gridApi;
                        //行首
                        vm.workCenterGridOptions.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 35, cellTemplate: 'etu-ui-grid/rowHeader' });
                        //分页
                        vm.workCenterGridOptions.gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindWorkCenters();
                        });
                    }
                }

                //绑定列表
                vm.bindWorkCenters = function () {
                    var searchInput = {
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        workCenterCode: '',
                        workCenterName: '',
                        orderNumber: '',
                    };
                    if (!!vm.filterText && vm.filterText !== '') {
                        if (vm.filterType === 'stWorkCenterCode')
                            searchInput.workCenterCode = vm.filterText;
                        else if (vm.filterType === 'stWorkCenterName')
                            searchInput.workCenterName = vm.filterText;
                        //else if (vm.filterType === 'stOrderNumber')
                        //    searchInput.orderNumber = vm.filterText;
                    }
                    vm.loading = true;
                    workcenterService
                        .findWorkCenterList(searchInput)
                        .then(function (result) {
                            vm.workCenterGridOptions.totalItems = result.data.totalCount;
                            vm.workCenterGridOptions.data = result.data.items;
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                //跳转到明细
                vm.showOrders = function (workItem) {
                    etuBreadcrumbsService.push({
                        label: '机台作业明细'
                    });
                    $state.go('dispatchedOrders.orders', { 'workCenterId': workItem.workCenterId });
                };

                vm.asyncPrepares = function () {
                    vm.loading = true;
                    var workInput = {
                        workerType: 'TimeTask',
                    };
                    dispatchedPrepareService
                    .doWorkForDispatched(workInput)
                    .then(function (result) { })
                    .finally(function () {
                        vm.loading = false;
                    });

                };

                ////同步订单
                //vm.syncOrders = function () {
                //    $uibModal.open({
                //        templateUrl: '~/App/Main/views/handovers/syncSapOrder.cshtml',
                //        controller: 'syncSapOrderController as vm',
                //        backdrop: 'static'
                //    });
                //};

                vm.bindWorkCenters();

            } //end controller
        ]);
})();