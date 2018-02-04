(function () {
    'use strict';

    angular.module('app.main')
        .controller('dispatchedOrdersController', [
            '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', 'appSessionService', 'etuBreadcrumbsService', 'abp.services.manufacture.dispatchedOrder', 'abp.services.manufacture.workCenter',
            function ($scope, $rootScope, $state, $stateParams, $uibModal, appSessionService, etuBreadcrumbsService, dispatchedOrderService, workCenterService) {

                $rootScope.setViewTitle('机台作业明细');  //设置页标题

                //$scope.gridOptions = {
                //    expandableRowTemplate: 'expandableRowTemplate.html',
                //    expandableRowHeight: 150,
                //    //subGridVariable will be available in subGrid scope
                //    expandableRowScope: {
                //        subGridVariable: 'subGridScopeVariable'
                //    }
                //}


                var vm = this;
                vm.currentOrganizationUnit = appSessionService.organizationUnit; //当前部门
                vm.formMode = 0; //0:浏览, 1:编辑, 2:接收处理
                vm.loading = false;
                vm.handOverDepartments = [];
                vm.workCenterId = $stateParams.workCenterId;
                vm.workCenter = {};
                vm.ordersGridOptions = [];
                vm.defaultPageSizes = 5;
                vm.defaultPageSize = 10;

                //返回列表页
                vm.gotoWorkCenters = function () {
                    etuBreadcrumbsService.pop(0);
                    $state.go('dispatchedOrders.workcenters');

                };

                vm.gotoDrawing = function (partNumber) {
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/orders/drawingView.cshtml',
                        controller: 'dispatchedDrawingController as vm',
                        backdrop: 'static',
                        windowClass: 'modal-full-width',
                        resolve: {
                            partNumber: function () {
                                if (!partNumber)
                                    return;
                                return partNumber;
                            }
                        }
                    });
                };

                vm.gotoCAPP = function (orderNumber) {
                    //工艺卡片
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/orders/cappView.cshtml',
                        controller: 'cappController as vm',
                        backdrop: 'static',
                        windowClass: 'modal-full-width',
                        resolve: {
                            orderNumber: function () {
                                return orderNumber;
                            }
                        }
                    });
                };


                vm.gotoTooling = function (partNumber) {
                    getArchive(partNumber, '1');
                };

                function getArchive(partNumber, archiveType) {
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/orders/cncArchiveView.cshtml',
                        controller: 'dispatchedCncArchiveController as vm',
                        backdrop: 'static',
                        windowClass: 'modal-full-width',
                        resolve: {
                            partNumber: function () {
                                if (!partNumber)
                                    return;
                                return partNumber;
                            },
                            archiveType: function () {
                                return archiveType;
                            }
                        }
                    });
                };

                vm.paginationOptions = {
                    pageSizes: vm.defaultPageSizes, //显示几页
                    pageSize: vm.defaultPageSize,  //一页几项
                    totalCount: 0,
                    pages: 0, //分页数
                    newPages: this.pages > this.pageSizes ? this.pageSizes : this.pages,
                    selPage: 1,
                    appScopeProvider: vm,
                    pageList: [],

                };

                function bindPage(page, total) {
                    vm.paginationOptions.totalCount = total;
                    vm.paginationOptions.pages = Math.ceil(total / vm.paginationOptions.pageSize); //分页数
                    vm.paginationOptions.newPages = vm.paginationOptions.pages > vm.paginationOptions.pageSizes ? vm.paginationOptions.pageSizes : vm.paginationOptions.pages;

                    bindPageList(page);


                };

                function bindPageList(selpage) {

                    ////不能小于1大于最大
                    //if (selpage < 1 || selpage > vm.paginationOptions.pages) return;

                    vm.paginationOptions.pageList = [];
                    var newpageList = [];
                    if (selpage >= 1 && selpage <= 2) {
                        //分页要repeat的数组
                        for (var i = 0; i < vm.paginationOptions.newPages; i++) {
                            newpageList.push(i + 1);
                        }
                    }
                    //最多显示分页数5
                    if (selpage > 2) {

                        //因为只显示5个页数，大于2页开始分页转换
                        var avgCountPre = Math.ceil(vm.paginationOptions.newPages / 2 - 1);
                        var avgCountNext = Math.floor(vm.paginationOptions.newPages / 2);
                        var minIndex = Math.max(1, selpage - avgCountPre);
                        var maxIndex = Math.min(vm.paginationOptions.pages, selpage + avgCountNext);
                        for (var i = minIndex ; i <= maxIndex ; i++) {
                            newpageList.push(i);
                        }


                        //for (var i = (selpage - 3) ; i < ((selpage + 2) > vm.paginationOptions.pages ? vm.paginationOptions.pages : (selpage + 2)) ; i++) {
                        //    newpageList.push(i + 1);
                        //}
                    }
                    vm.paginationOptions.pageList = newpageList;

                    ////分页要repeat的数组
                    //for (var i = 0; i < vm.paginationOptions.newPages; i++) {
                    //    vm.paginationOptions.pageList.push(i + 1);
                    //}
                };

                //打印当前选中页索引
                vm.selectPage = function (page) {
                    //不能小于1大于最大
                    if (page < 1 || page > vm.paginationOptions.pages) return;

                    vm.paginationOptions.selPage = page;
                    bindOrders();
                    vm.isActivePage(page);
                    //console.log("选择的页：" + page);
                };

                //设置当前选中页样式
                vm.isActivePage = function (page) {
                    return vm.paginationOptions.selPage == page;
                },
                //上一页
                vm.previous = function () {
                    vm.selectPage(vm.paginationOptions.selPage - 1);
                },
                //下一页
                vm.next = function () {
                    vm.selectPage(vm.paginationOptions.selPage + 1);
                }

                vm.getTotalWorkTime = function (auxi, work, quantity) {
                    if (!quantity) return 0;
                    return (auxi * 1 + work * quantity).toFixed(2);
                }

                //取得选中的行
                function getSelectedBillLineIds() {
                    return _.chain(vm.billLinesData)
                        .filter(function (billLine) {
                            return billLine.$checked;
                        })
                        .map(function (billLine) {
                            return billLine.id;
                        })
                        .value();
                };


                //绑定列表
                function bindOrders() {
                    var searchInput = {
                        skipCount: (vm.paginationOptions.selPage - 1) * vm.paginationOptions.pageSize,
                        maxResultCount: vm.paginationOptions.pageSize,
                        workCenterId: vm.workCenterId
                    };
                    vm.ordersGridOptions = [];
                    vm.loading = true;
                    dispatchedOrderService
                        .findDispatchOrderPagerByWorkCenter(searchInput)
                        .then(function (result) {
                            vm.ordersGridOptions = result.data.items;

                            bindPage(vm.paginationOptions.selPage, result.data.totalCount);
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                function bindWorkCenter() {
                    workCenterService
                        .getWorkCenter(vm.workCenterId)
                        .then(function (result) {
                            vm.workCenter = result.data;
                        });
                }

                //工具
                vm.util = {
                    formatOrderLine: function (orderInfo) {
                        if (orderInfo) {
                            if (orderInfo.sourceName === 'SAP') {
                                return orderInfo.orderNumber;
                            }
                            else if (orderInfo.sourceName === 'FS') {
                                return orderInfo.orderNumber + '/' + orderInfo.lineNumber;
                            }
                        }
                        return '';
                    },
                    selectAll: function (singal) {
                        _.forEach(vm.billLinesData, function (billLine) {
                            if (singal === true && billLine.lineState === 0)
                                billLine.$checked = true;
                            else
                                billLine.$checked = false;
                        });
                    }
                };

                (function init() {
                    vm.loading = true;
                    bindWorkCenter();
                    bindOrders();

                })();

            }
        ]);
})();