(function () {
    'use strict';

    angular.module('app.main')
        .controller('handOverBillListController', [
            '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', 'etuBreadcrumbsService', 'abp.services.manufacture.handOver',
            function ($scope, $rootScope, $state, $stateParams, $uibModal, etuBreadcrumbsService, handOverService) {

                $rootScope.setViewTitle('交接单查询');  //设置页标题

                var vm = this;
                vm.loading = false;
                vm.filterType = 'stBillCode';
                vm.filterText = null;
                vm.searchRangeBegin = null; //new Date(moment().startOf('month'));
                vm.searchRangeEnd = null; //new Date(moment().startOf('day'));
                vm.stateList = [
                    { name: '草稿', value: 0, checked: true },
                    { name: '转出', value: 1, checked: true },
                    { name: '完成', value: 2 }
                ];
                vm.getCheckedState = function () {
                    return _.chain(vm.stateList)
                        .filter(function (state) { return state.checked; })
                        .map(function (state) { return state.value; })
                        .value();
                };

                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize
                };

                vm.searchRangeBeginDateOptions = {
                    startingDay: 1,
                    showWeeks: false,
                    isOpened: false
                };
                vm.openSearchRangeBegin = function () {
                    vm.searchRangeBeginDateOptions.isOpened = true;
                };

                vm.searchRangeEndDateOptions = {
                    startingDay: 1,
                    showWeeks: false,
                    isOpened: false
                };
                vm.openSearchRangeEnd = function () {
                    vm.searchRangeEndDateOptions.isOpened = true;
                };

                //交接单单列表
                vm.handOverBillGridOptions = {
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
                        name: '交接单号',
                        field: 'billCode',
                        width: 100
                    }, {
                        name: '转出部门',
                        field: 'transferSource.organizationUnitName',
                        width: 100
                    }, {
                        name: '转入部门/供方',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span ng-if=\"row.entity.transferTargetType==0\">{{row.entity.transferTargetDepartment.organizationUnitName}}</span>' +
                            '  <span ng-if=\"row.entity.transferTargetType==1\">{{row.entity.transferTargetSupplier.supplierFullName}}</span>' +
                            '</div>',
                        width: 180
                    }, {
                        name: '状态',
                        field: 'billState',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '  <span ng-class=\"{\'red\': row.entity.billState==0, \'green\': row.entity.billState!=0}\">{{row.entity.billStateName}}</span>' +
                            '</div>',
                        width: 80
                    }, {
                        name: '创建人',
                        field: 'creatorUserName',
                        width: 100
                    }, {
                        name: '送出时间',
                        field: 'handOverDate',
                        cellFilter: 'momentFormat: \'L\'',
                        width: 95
                    }, {
                        name: '行状态',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span>共{{row.entity.totalLineCount}}条;</span>' +
                            '  <span>接收{{row.entity.receivedLineCount}}条;</span>' +
                            '  <span>退回{{row.entity.rejectedLineCount}}条;</span>' +
                            '  <span>转送{{row.entity.transferLineCount}}条;</span>' +
                            '  <span>未处理{{row.entity.pendingLineCount}}条;</span>' +
                            '</div>',
                        width: 350
                    }, {
                        name: 'rowOperator',
                        displayName: '',
                        enableSorting: false,
                        width: 85,
                        pinnedRight: true,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '   <button class=\"btn btn-xs btn-flat bg-silver blue-stripe\" ng-click="grid.appScope.showBillDetail(row.entity)">' + '<i class=\"fa fa-fw fa-history\"></i>明细' + '</button>' +
                            '</div>'
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.handOverBillGridOptions.gridApi = gridApi;
                        //行首
                        vm.handOverBillGridOptions.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 35, cellTemplate: 'etu-ui-grid/rowHeader' });
                        //分页
                        vm.handOverBillGridOptions.gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindBills();
                        });
                    }
                }

                //绑定列表
                vm.bindBills = function () {
                    var searchInput = {
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        rangeBegin: vm.searchRangeBegin,
                        rangeEnd: vm.searchRangeEnd,
                        state: vm.getCheckedState()
                    };
                    if (vm.filterType === 'stBillCode')
                        searchInput.billCode = vm.filterText;
                    else if (vm.filterType === 'stOrderNumber')
                        searchInput.orderNumber = vm.filterText;
                    else if (vm.filterType === 'stItemNumber')
                        searchInput.itemNumber = vm.filterText;
                    else if (vm.filterType === 'stTransferSourceName')
                        searchInput.transferSourceName = vm.filterText;
                    else if (vm.filterType === 'stTransferTargetName')
                        searchInput.transferTargetName = vm.filterText;

                    vm.loading = true;
                    handOverService
                        .findHandOverBills(searchInput)
                        .then(function (result) {
                            vm.handOverBillGridOptions.totalItems = result.data.totalCount;
                            vm.handOverBillGridOptions.data = result.data.items;
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                //跳转到明细
                vm.showBillDetail = function (billItem) {
                    etuBreadcrumbsService.push({
                        label: '交接单明细'
                    });
                    $state.go('handOverBills.detail', { 'billId': billItem.id });
                };

                //创建交接单
                vm.createBill = function() {
                    vm.loading = true;
                    handOverService
                        .createHandOverBill()
                        .then(function (result) {
                            var billItem = result.data;
                            etuBreadcrumbsService.push({
                                label: '交接单明细'
                            });
                            $state.go('handOverBills.detail', { 'billId': billItem.id });
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                //同步订单
                vm.syncOrders = function() {
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/handovers/syncSapOrder.cshtml',
                        controller: 'syncSapOrderController as vm',
                        backdrop: 'static'
                    });
                };

                vm.bindBills();

            } //end controller
        ]);
})();