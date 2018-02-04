(function () {
    'use strict';

    angular.module('app.main')
        .controller('editCooperateController', [
            '$scope', '$uibModalInstance', 'abp.services.manufacture.processManage', 'cooperateId',
            function ($scope, $uibModalInstance, processManageService, cooperateId) {
                var vm = this;
                vm.loading = false;
                vm.cooperateId = cooperateId;
                vm.cooperateProcess = null;

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.save = function () {
                    vm.loading = true;
                    processManageService
                        .updateSapOrderProcessCooperate({
                            cooperateId: vm.cooperateProcess.cooperateId,
                            cooperateType: vm.cooperateProcess.cooperateType,
                            cooperaterCode: vm.cooperateProcess.cooperaterCode,
                            cooperaterName: vm.cooperateProcess.cooperaterName,
                            cooperaterPrice: vm.cooperateProcess.cooperaterPrice
                        })
                        .then(function () {
                            abp.notify.success('工艺保存成功！');
                            $uibModalInstance.close();
                        })
                        .finally(function () {
                            vm.loading = true;
                        });
                };

                (function init() {
                    vm.loading = false;
                    processManageService
                        .getSapOrderProcessCooperater(vm.cooperateId)
                        .then(function (result) {
                            vm.cooperateProcess = result.data;
                        })
                        .finally(function () {
                            vm.loading = true;
                        });
                })();
            }
        ]);
})();