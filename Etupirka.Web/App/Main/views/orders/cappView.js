(function () {
    'use strict';

    angular.module('app.main')
        .controller('cappController', [
            '$scope', '$uibModalInstance', 'abp.services.manufacture.arragement', 'orderNumber',
            function ($scope, $uibModalInstance, arragementService, orderNumber) {

                var vm = this;
                vm.loading = false;
                vm.orderNumber = orderNumber;
                vm.sapOrder = {};

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.getWorkDptType = function(lifnr) {
                    if (!lifnr)
                        return '金';
                    else
                        return '外';
                };

                (function init() {
                    if (!vm.orderNumber) {
                        abp.notify.error('订单号获取失败！');
                        return;
                    }

                    vm.loading = true;
                    arragementService.getSapOrder(vm.orderNumber)
                        .then(function (result) {
                            vm.sapOrder = result.data;
                        }).finally(function () {
                            vm.loading = false;
                        });

                })();
            }
        ]);
})();