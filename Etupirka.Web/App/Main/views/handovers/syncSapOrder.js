(function () {
    'use strict';

    angular.module('app.main')
        .controller('syncSapOrderController', [
            '$scope', '$uibModalInstance', 'abp.services.manufacture.cooperate',
            function ($scope, $uibModalInstance, cooperateService) {
                var vm = this;
                vm.loading = false;
                vm.orderNumberBegin = null;
                vm.orderNumberEnd = null;

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.syncSAPOrder = function () {
                    abp.message.confirm('确定要同步SAP订单吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.loading = true;
                            var input = {
                                orderNumberRangeBegin: vm.orderNumberBegin,
                                orderNumberRangeEnd: vm.orderNumberEnd
                            };
                            cooperateService
                                .sapMOrderSync(input)
                                .then(function () {
                                    abp.notify.success('SAP订单同步成功！');
                                    $uibModalInstance.close();
                                })
                                .finally(function () {
                                    vm.loading = false;
                                });
                        });
                };

            }
        ]);
})();