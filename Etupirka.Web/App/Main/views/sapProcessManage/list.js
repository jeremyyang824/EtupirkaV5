(function () {
    'use strict';

    angular.module('app.main')
        .controller('sapProcessListController', [
            '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', 'appFileDownload', 'etuBreadcrumbsService', 'abp.services.manufacture.processManage',
            function ($scope, $rootScope, $state, $stateParams, $uibModal, appFileDownload, etuBreadcrumbsService, processManageService) {

                $rootScope.setViewTitle('SAP订单工艺');  //设置页标题

                var vm = this;
                vm.loading = false;
                vm.orderNumberRangeBegin = null;
                vm.orderNumberRangeEnd = null;

                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize
                };

                //工艺列表
                vm.sapProcessGridOptions = {
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
                        name: '订单号',
                        field: 'orderNumber',
                        width: 100
                    }, {
                        name: 'MRP控制',
                        field: 'mrpController',
                        width: 100
                    }, {
                        name: '物料编码',
                        field: 'materialNumber',
                        width: 120
                    }, {
                        name: '物料名称',
                        field: 'materialDescription',
                        width: 120
                    }, {
                        name: '订单数量',
                        field: 'targetQuantity',
                        width: 80
                    }, {
                        name: 'WBS元素',
                        field: 'wbsElement',
                        width: 100
                    }, {
                        name: '工序号',
                        field: 'operationNumber',
                        width: 80
                    }, {
                        name: '控制码',
                        field: 'operationCtrlCode',
                        width: 120
                    }, {
                        name: '工作中心',
                        field: 'workCenterCode',
                        width: 120
                    }, {
                        name: '准备工时',
                        field: 'vgw01',
                        width: 80
                    }, {
                        name: '机器工时',
                        field: 'vgw02',
                        width: 80
                    }, {
                        name: '人工工时',
                        field: 'vgw03',
                        width: 80
                    }, {
                        name: '外协类型',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents text-center\">' +
                            '  <span ng-class=\"{\'blue\': row.entity.cooperateType==0, \'orange\': row.entity.cooperateType==1}\">{{row.entity.cooperateTypeName}}</span>' +
                            '</div>',
                        width: 80
                    }, {
                        name: '供方代码',
                        field: 'cooperaterCode',
                        width: 80
                    }, {
                        name: '供应商名',
                        field: 'cooperaterName',
                        width: 120
                    }, {
                        name: '外协价格',
                        field: 'cooperaterPrice',
                        width: 80
                    }, {
                        name: 'rowOperator',
                        displayName: '',
                        enableSorting: false,
                        width: 110,
                        pinnedRight: true,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '   <button class=\"btn btn-xs btn-flat bg-silver blue-stripe\" ng-if=\"row.entity.cooperateId\" ng-click=\"grid.appScope.editCooperate(row.entity)\">' + '<i class=\"fa fa-fw fa-pencil\"></i>编辑外协' + '</button>' +
                            '</div>'
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.sapProcessGridOptions.gridApi = gridApi;
                        //行首
                        vm.sapProcessGridOptions.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 35, cellTemplate: 'etu-ui-grid/rowHeader' });
                        //分页
                        vm.sapProcessGridOptions.gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindSapProcess();
                        });
                    }
                }

                //绑定列表
                vm.bindSapProcess = function () {
                    var searchInput = {
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        orderNumberRangeBegin: vm.orderNumberRangeBegin,
                        orderNumberRangeEnd: vm.orderNumberRangeEnd
                    };

                    vm.loading = true;
                    processManageService
                        .getSapOrderProcessWithCooperaterPager(searchInput)
                        .then(function (result) {
                            vm.sapProcessGridOptions.totalItems = result.data.totalCount;
                            vm.sapProcessGridOptions.data = result.data.items;
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                //导出工艺
                vm.outputSapProcess = function () {
                    if (!vm.orderNumberRangeBegin || !vm.orderNumberRangeEnd) {
                        var alertMsg = '确定导出所有订单工艺吗？';
                        if (vm.orderNumberRangeBegin)
                            alertMsg = '确定导出[' + vm.orderNumberRangeBegin + ']及之后的订单吗？';
                        if (vm.orderNumberRangeEnd)
                            alertMsg = '确定导出[' + vm.orderNumberRangeEnd + ']及之前的订单吗？';

                        abp.message.confirm(alertMsg)
                            .then(function (isConfirmed) {
                                if (!isConfirmed)
                                    return;

                                doOutputSapProcess();
                            });
                    } else {
                        doOutputSapProcess();
                    }

                    function doOutputSapProcess() {
                        abp.notify.info("开始导出Excel，请稍等...");

                        vm.loading = true;
                        processManageService
                            .getSapOrderProcessWithCooperaterExcel({
                                orderNumberRangeBegin: vm.orderNumberRangeBegin,
                                orderNumberRangeEnd: vm.orderNumberRangeEnd
                            })
                            .then(function (result) {
                                //下载文件
                                appFileDownload.downloadTempFile(result.data);
                            })
                            .finally(function () {
                                vm.loading = false;
                            });
                    }
                };

                //导入工艺
                vm.importSapProcess = function() {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/sapProcessManage/importProcess.cshtml',
                        controller: 'importProcessController as vm',
                        backdrop: 'static'
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindSapProcess();
                    });
                };

                //编辑工艺
                vm.editCooperate = function(line) {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/sapProcessManage/editCooperate.cshtml',
                        controller: 'editCooperateController as vm',
                        backdrop: 'static',
                        resolve: {
                            cooperateId: function () {
                                return line.cooperateId;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        vm.bindSapProcess();
                    });
                };

                //同步订单
                vm.syncOrders = function () {
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/handovers/syncSapOrder.cshtml',
                        controller: 'syncSapOrderController as vm',
                        backdrop: 'static'
                    });
                };

                vm.bindSapProcess();

            } //end controller
        ]);
})();