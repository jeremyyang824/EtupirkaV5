(function () {
    'use strict';

    angular.module('app.main')
        .controller('handOverBillLineAddSapController', [
            '$scope', '$uibModalInstance', 'abp.services.manufacture.handOver', 'abp.services.manufacture.sapMOrder',
            'billId', 'inputOrderInfo', 'transferInfo',
            function ($scope, $uibModalInstance, handOverService, sapMOrderService, billId, inputOrderInfo, transferInfo) {

                var canCooperCtrlCodes = ['ZQ02', 'PP02', 'PP06'];

                var vm = this;
                vm.loading = false;
                vm.billId = billId;
                vm.inputOrderInfo = inputOrderInfo;
                vm.transferInfo = transferInfo;
                vm.sapOrder = null;
                vm.sapProcessList = [];
                vm.remark = '';

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                //该道序是否可添加到交接单
                vm.canProcessHandOver = function (sapProcess) {
                    var nextSapProcess = null;
                    var idx = _.indexOf(vm.sapProcessList, sapProcess);
                    if (idx >= 0 && idx < vm.sapProcessList.length - 1) {
                        nextSapProcess = vm.sapProcessList[idx + 1];
                    }

                    //末道
                    if (nextSapProcess == null)
                        return true;

                    if (vm.transferInfo.transferSource.organizationUnitCode == 35) {
                        //西厂区转出
                        return sapProcess.cooperateType == null && nextSapProcess.cooperateType != null;
                    } else {
                        //非西厂区转出
                        return sapProcess.cooperateType != null;
                    }
                };

                //选择交接道序
                vm.selectSapProcess = function (sapProcess) {
                    vm.loading = true;
                    handOverService
                        .addSapHandOverBillLine({
                            billId: vm.billId,
                            sapMOrderId: vm.sapOrder.id,
                            sapMOrderProcessId: !sapProcess ? null : sapProcess.id, //首道交接时为null
                            handOverQuantity: vm.sapOrder.targetQuantity,
                            remark: vm.remark
                        })
                        .then(function (result) {
                            abp.notify.info('交接行添加成功！');
                            $uibModalInstance.close();
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                }

                //绑定订单信息
                function bindOrder() {
                    return sapMOrderService
                        .findSapMOrderByOrderNumber(vm.inputOrderInfo.orderNumber)
                        .then(function (result) {
                            vm.sapOrder = result.data;
                        });
                };

                //绑定工艺列表
                function bindProcessList() {
                    return sapMOrderService
                        .getSapMOrderProcessListWithCooperate(vm.sapOrder.id)
                        .then(function (result) {
                            vm.sapProcessList = result.data.items;
                        });
                }

                (function init() {
                    if (vm.inputOrderInfo.sourceName !== 'SAP') {
                        abp.notify.error('信息读取失败！');
                        return;
                    }

                    vm.loading = true;
                    bindOrder()
                        .then(bindProcessList)
                        .finally(function () {
                            vm.loading = false;
                        });
                })();
            }
        ]);
})();