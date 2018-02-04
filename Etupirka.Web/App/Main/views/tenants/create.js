(function () {
    'use strict';

    angular.module('app.main')
        .controller('tenantCreateController', [
            '$scope', '$uibModalInstance', '$timeout', 'abp.services.portal.tenant',
            function ($scope, $uibModalInstance, $timeout, tenantService) {
                var vm = this;

                vm.saving = false;
                vm.tenant = {
                    tenancyName: '',
                    name: '',
                    adminEmailAddress: ''
                };

                //保存创建租户
                vm.save = function () {
                    abp.message.confirm('确定要创建新的烟厂吗？烟厂信息创建后不可删除！')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.saving = true;
                            tenantService.createTenant(vm.tenant)
                                .success(function () {
                                    abp.notify.success('创建成功！');
                                    $uibModalInstance.close();
                                })
                                .finally(function () {
                                    vm.saving = false;
                                });
                        });
                };

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

            }
        ]);
})();