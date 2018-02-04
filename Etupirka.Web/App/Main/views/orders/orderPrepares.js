(function () {
    'use strict';

    angular.module('app.main')
        .controller('dispatchedOrderPreparesController', [
            '$scope', '$rootScope', '$state', 'abp.services.manufacture.dispatchedPrepare',
            function ($scope, $rootScope, $state, dispatchPrepareService) {

                $rootScope.setViewTitle('机台作业齐备性');  //设置页标题

                var vm = this;
                vm.loading = false;
                vm.jobType = 'tl';   //齐备性流程类型{name:'全部',value:'all'},
                vm.jobStatus = '1';  //齐备性流程状态

                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize
                };

                //交接单单列表
                vm.prepareGridOptions = {
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
                        name: '类型',
                        field: 'stepTransactionType',
                        width: 50
                    }, {
                        name: '生产订单号',
                        field: 'orderNumber',
                        width: 100
                    }, {
                        name: '道序号',
                        field: 'routingNumber',
                        width: 50
                    }, {
                        name: '物料号',
                        field: 'materialNumber',
                        width: 120
                    }, {
                        name: '物料描述',
                        field: 'materialDescription',
                        width: 150
                    }, {
                        name: '工作中心编码',
                        field: 'actualWorkID',
                        width: 100
                    }, {
                        name: '工作中心描述',
                        field: 'actualWorkName',
                        width: 150
                    }, {
                        name: '呼叫时间',
                        field: 'stepStartedDate',
                        width: 150
                    }, {
                        name: '要求完成时间',
                        field: 'stepRequiredDate',
                        width: 150
                    }, {
                        name: '完成时间',
                        field: 'stepFinishedDate',
                        width: 150
                    }, {
                        name: '呼叫状态',
                        field: 'stepStatusStr',
                        width: 70
                    }, {
                        name: '是否超期',
                        field: 'stepDelayed',
                        width: 100
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.prepareGridOptions.gridApi = gridApi;
                        //行首
                        vm.prepareGridOptions.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 35, cellTemplate: 'etu-ui-grid/rowHeader' });
                        //分页
                        vm.prepareGridOptions.gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindParepares();
                        });
                    }
                }

                //绑定列表
                vm.bindParepares = function () {
                    var searchInput = {
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        prepareType: vm.jobType,
                        prepareStatus: vm.jobStatus
                        //orderNumber: '',
                    };

                    vm.loading = true;
                    dispatchPrepareService
                        .findPrepareInfos_NC(searchInput)
                        .then(function (result) {
                            vm.prepareGridOptions.totalItems = result.data.totalCount;
                            vm.prepareGridOptions.data = result.data.items;
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                vm.bindParepares();

            } //end controller
        ]);
})();