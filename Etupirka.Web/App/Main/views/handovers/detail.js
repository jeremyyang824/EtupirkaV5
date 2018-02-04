(function () {
    'use strict';

    angular.module('app.main')
        .controller('handOverBillDetailController', [
            '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', '$q', 'appSessionService', 'etuBreadcrumbsService', 'abp.services.manufacture.handOver', 'abp.services.manufacture.cooperate',
            function ($scope, $rootScope, $state, $stateParams, $uibModal, $q, appSessionService, etuBreadcrumbsService, handOverService, cooperateService) {

                $rootScope.setViewTitle('交接单明细');  //设置页标题

                var vm = this;
                vm.currentOrganizationUnit = appSessionService.organizationUnit; //当前部门
                vm.formMode = 0; //0:浏览, 1:编辑, 2:接收处理
                vm.loading = false;
                vm.handOverDepartments = [];
                vm.billId = $stateParams.billId;
                vm.billData = null;
                vm.billLinesData = [];
                //录入交接单行信息
                vm.inputOrderInfo = {
                    sourceName: 'FS',
                    formatOrderLine: '',
                };

                //返回列表页
                vm.gotoList = function () {
                    etuBreadcrumbsService.pop(0);
                    $state.go('handOverBills.list');
                }

                //保存交接单
                vm.saveBill = function () {
                    vm.loading = true;
                    handOverService
                        .saveHandOverBill(vm.billData)
                        .then(function () {
                            abp.notify.info('交接单保存成功！');
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

                //撤销交接单
                vm.deleteBill = function () {
                    abp.message.confirm('确定要撤销该交接单吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.loading = true;
                            handOverService
                                .deleteHandOverBill(vm.billData.id)
                                .then(function () {
                                    etuBreadcrumbsService.pop(0);
                                    $state.go('handOverBills.list');
                                    abp.notify.success('交接单删除成功！');
                                })
                                .finally(function () {
                                    vm.loading = false;
                                });
                        });
                };

                //提交交接单
                vm.publishBill = function () {
                    abp.message.confirm('确定要提交该交接单吗？')
                       .then(function (isConfirmed) {
                           if (!isConfirmed)
                               return;

                           vm.loading = true;
                           handOverService
                               .publishHandOverBill(vm.billData)
                               .then(function (result) {
                                   if (result.data) {
                                       $state.go('handOverBills.list');
                                       abp.notify.success('交接单提交成功！');
                                   } else {
                                       abp.notify.error('交接单提交失败！');
                                   }
                               })
                               .finally(function () {
                                   vm.loading = false;
                               });
                       });
                };

                //删除交接单行
                vm.deleteBillLine = function (line) {
                    if (line && line.id) {
                        abp.message.confirm('确定要删除该领用物料吗？')
                           .then(function (isConfirmed) {
                               if (!isConfirmed)
                                   return;

                               bindBillLines();
                               abp.notify.info('删除成功！');
                           });
                    }
                };

                //编辑交接单行
                vm.editBillLine = function (line) {
                    var modalInstance = $uibModal.open({
                        templateUrl: '~/App/Main/views/receivebills/editLine.cshtml',
                        controller: 'receiveBillLineEditController as vm',
                        backdrop: 'static',
                        resolve: {
                            billLineData: function () {
                                return line;
                            }
                        }
                    });

                    modalInstance.result.then(function (result) {
                        bindBillLines();
                    });
                };

                //是否已经选定了转出部门
                vm.isBillLineAddEnable = function () {
                    return (vm.billData.transferTargetType === 0 && vm.billData.transferTargetDepartment && vm.billData.transferTargetDepartment.organizationUnitCode)
                        || (vm.billData.transferTargetType === 1 && vm.billData.transferTargetSupplier && vm.billData.transferTargetSupplier.supplierCode);
                };

                //添加交接行
                vm.billLineAdd = function () {
                    //格式化订单号/行号
                    if (!vm.inputOrderInfo.formatOrderLine) {
                        abp.notify.error('请输入订单号(/行号)');
                        return;
                    }

                    var addLineController = null;
                    var addLineViewUrl = null;
                    if (vm.inputOrderInfo.sourceName === 'FS') {
                        var tempArr = vm.inputOrderInfo.formatOrderLine.split('/');
                        if (tempArr.length !== 2 || tempArr[0] == '' || tempArr[1] == '') {
                            abp.notify.error('FS订单请输入\'订单号/行号\'');
                            return;
                        }
                        vm.inputOrderInfo.orderNumber = tempArr[0];
                        vm.inputOrderInfo.lineNumber = tempArr[1];

                        addLineController = 'handOverBillLineAddFsController as vm';
                        addLineViewUrl = '~/App/Main/views/handovers/addFsLine.cshtml';
                    }
                    else if (vm.inputOrderInfo.sourceName === 'SAP') {
                        vm.inputOrderInfo.orderNumber = vm.inputOrderInfo.formatOrderLine;
                        vm.inputOrderInfo.lineNumber = null;

                        addLineController = 'handOverBillLineAddSapController as vm';
                        addLineViewUrl = '~/App/Main/views/handovers/addSapLine.cshtml';
                    }
                    else {
                        abp.notify.error('未识别的订单来源！');
                        return;
                    }

                    //首次添加交接行时先保存交接单数据
                    if (vm.billLinesData.length === 0) {
                        vm.saveBill();
                    }

                    var deferred = $q.defer();
                    vm.loading = true;
                    //添加SAP交接单行时先同步SAP订单
                    if (vm.inputOrderInfo.sourceName === 'SAP') {
                        var input = {
                            orderNumberRangeBegin: vm.inputOrderInfo.orderNumber,
                            orderNumberRangeEnd: vm.inputOrderInfo.orderNumber
                        };
                        cooperateService
                            .sapMOrderSync(input)
                            .then(function () {
                                deferred.resolve(true);
                                abp.notify.success('SAP订单同步成功！');
                            })
                            .catch(function() {
                                deferred.reject(false);
                            })
                            .finally(function() {
                                vm.loading = false;
                            });
                    } else {
                        deferred.resolve(true);
                        vm.loading = false;
                    }

                    deferred.promise.then(function (isSuccess) {
                        //打开道序选择窗口
                        $uibModal.open({
                            templateUrl: addLineViewUrl,
                            controller: addLineController,
                            backdrop: 'static',
                            size: 'lg',
                            resolve: {
                                billId: function () {
                                    return vm.billId;
                                },
                                inputOrderInfo: function () {
                                    return vm.inputOrderInfo;
                                },
                                transferInfo: function () {
                                    return {
                                        transferSource: vm.billData.transferSource,
                                        transferTargetType: vm.billData.transferTargetType,
                                        transferTargetDepartment: vm.billData.transferTargetDepartment,
                                        transferTargetSupplier: vm.billData.transferTargetSupplier
                                    }
                                }
                            }
                        }).result.then(function (result) {
                            bindBillLines();
                        });
                    });

                };

                //删除交接行
                vm.billLineDeleteMuti = function () {
                    var selIds = getSelectedBillLineIds();
                    abp.message.confirm('确定要删除选中交接单行吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.loading = true;
                            handOverService
                                .deleteHandOverBillLines(vm.billData.id, selIds)
                                .then(function () {
                                    bindBillLines();
                                    abp.notify.success('交接单行删除成功！');
                                })
                                .finally(function () {
                                    vm.loading = false;
                                });
                        });
                };

                //接收（选中）交接行
                vm.billLineReceiveMuti = function () {
                    var selIds = getSelectedBillLineIds();
                    abp.message.confirm('确定要接收选中交接单行吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.loading = true;
                            handOverService
                                .receiveHandOverBillLines(vm.billData.id, selIds)
                                .then(function (result) {
                                    var isAllComplete = result.data;
                                    isAllComplete ? bindBill() : bindBillLines();
                                    abp.notify.success('交接单行接收成功！');
                                })
                                .finally(function () {
                                    vm.loading = false;
                                });
                        });
                };

                //退回（选中）交接行
                vm.billLineRejectMuti = function () {
                    var selIds = getSelectedBillLineIds();
                    abp.message.confirm('确定要退回选中交接单行吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.loading = true;
                            handOverService
                                .rejectHandOverBillLines(vm.billData.id, selIds)
                                .then(function (result) {
                                    var isAllComplete = result.data;
                                    isAllComplete ? bindBill() : bindBillLines();
                                    abp.notify.success('交接单行退回成功！');
                                })
                                .finally(function () {
                                    vm.loading = false;
                                });
                        });
                };

                //转送（选中）交接行
                vm.billLineTransferMuti = function () {

                };

                //显示该行接口日志
                vm.showSapLineLogs = function (billLine) {
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/handovers/sapCooperateLog.cshtml',
                        controller: 'sapCooperateLogController as vm',
                        backdrop: 'static',
                        windowClass: 'modal-full-width',
                        resolve: {
                            sapOrderNumber: function () {
                                return billLine.orderInfo.orderNumber;
                            }
                        }
                    });
                };

                vm.printBill = function () {
                    var url = abp.toAbsAppPath("/HandOver/Print/" + vm.billId);
                    window.open(url);
                };

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

                function bindBill() {
                    return handOverService
                        .getHandOverBill(vm.billId)
                        .then(function (result) {
                            vm.billData = result.data;
                        })
                        /*绑定明细行*/
                        .then(bindBillLines)
                        /*判断表单状态*/
                        .then(function () {
                            if (vm.currentOrganizationUnit) {
                                if (vm.billData.billState === 0) {  //草稿
                                    if (vm.billData.transferSource
                                        && vm.billData.transferSource.organizationUnitId === vm.currentOrganizationUnit.id) {
                                        vm.formMode = 1; //编辑状态
                                    }
                                }
                                else if (vm.billData.billState === 1) { //已转出
                                    if (vm.billData.transferTargetType === 1 //供方交接
                                        && vm.billData.transferSource
                                        /*&& vm.billData.transferSource.organizationUnitId === vm.currentOrganizationUnit.id*/) {
                                        vm.formMode = 2; //接收处理
                                    }
                                    if (vm.billData.transferTargetType === 0 //部门交接
                                        && vm.billData.transferTargetDepartment
                                        /*&& vm.billData.transferTargetDepartment.organizationUnitId === vm.currentOrganizationUnit.id*/) {
                                        vm.formMode = 2; //接收处理
                                    }
                                }
                                else if (vm.billData.billState === 2) { //已完成
                                    vm.formMode = 0; //浏览
                                }
                            }
                        });
                }

                function bindBillLines() {
                    return handOverService
                        .getHandOverBillLines(vm.billId)
                        .then(function (result) {
                            vm.billLinesData = result.data.items;
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
                    bindBill()
                        .then(function () {
                            //绑定交接部门列表
                            return handOverService
                                .getAllHandOverDepartments()
                                .then(function (result) {
                                    vm.handOverDepartments = result.data.items;
                                })
                                /*设置转入转出部门下拉框*/
                                .then(function () {
                                    if (vm.handOverDepartments.length > 0) {
                                        //转入部门下拉
                                        var sourceDptRef = _.find(vm.handOverDepartments, function (department) {
                                            return department.organizationUnitId === vm.billData.transferSource.organizationUnitId;
                                        });
                                        vm.billData.transferSource = sourceDptRef;

                                        //转出部门下拉
                                        if (vm.billData.transferTargetDepartment) {
                                            var targetDptRef = _.find(vm.handOverDepartments, function (department) {
                                                return department.organizationUnitId === vm.billData.transferTargetDepartment.organizationUnitId;
                                            });
                                            if (targetDptRef)
                                                vm.billData.transferTargetDepartment = targetDptRef;
                                        } else {
                                            vm.billData.transferTargetDepartment = vm.handOverDepartments[0];
                                        }
                                    }
                                });
                        })
                        .finally(function () {
                            vm.loading = false;
                        });

                })();

            }
        ]);
})();