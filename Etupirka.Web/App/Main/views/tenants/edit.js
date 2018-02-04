(function () {
    'use strict';

    angular.module('app.main')
        .controller('tenantEditController', [
            '$scope', '$uibModalInstance', 'abp.services.portal.tenant', 'tenantId',
            function ($scope, $uibModalInstance, tenantService, tenantId) {
                var vm = this;

                vm.saving = false;
                vm.tenant = null;

                //保存烟厂信息修改
                vm.save = function () {
                    vm.saving = true;
                    tenantService.updateTenant({
                        tenantId: vm.tenant.id,
                        tenancyName: vm.tenant.tenancyName,
                        name: vm.tenant.name,
                        isActive: vm.tenant.isActive
                    }).success(function () {
                        abp.notify.success('保存成功！');
                        $uibModalInstance.close();
                    }).finally(function () {
                        vm.saving = false;
                    });
                };

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                function init() {
                    tenantService.getTenant(tenantId)
                        .success(function (result) {
                            vm.tenant = result;
                        });
                }

                init();
            }
        ]);
})();