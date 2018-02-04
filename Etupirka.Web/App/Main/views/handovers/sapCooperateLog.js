(function () {
    'use strict';

    angular.module('app.main')
        .controller('sapCooperateLogController', [
            '$scope', '$uibModalInstance', 'abp.services.manufacture.cooperate', 'sapOrderNumber',
            function ($scope, $uibModalInstance, cooperateService, sapOrderNumber) {

                var vm = this;
                vm.loading = false;
                vm.sapOrderNumber = sapOrderNumber;
                vm.sapCooperateLogs = [];

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.toggleLogDetail = function (logItem) {
                    logItem.$isShowDetail = !logItem.$isShowDetail;
                }

                vm.formatStepStatus = function (isFinished) {
                    if (isFinished === null || isFinished === undefined)
                        return '';
                    if (isFinished === true)
                        return '成功';
                    else
                        return '失败';
                }

                //绑定日志信息
                function bindLogs() {
                    return cooperateService
                        .getSapMOrderCooperLogs(vm.sapOrderNumber)
                        .then(function (result) {
                            vm.sapCooperateLogs = result.data.items;
                        });
                }

                (function init() {
                    if (!vm.sapOrderNumber) {
                        abp.notify.error('SAP订单号获取失败！');
                        return;
                    }

                    vm.loading = true;
                    bindLogs()
                        .finally(function () {
                            vm.loading = false;
                        });

                })();
            }
        ]);
})();