(function () {
    'use strict';

    angular.module('app.main')
        .controller('roleCreateController', [
            '$scope', '$uibModalInstance', '$timeout', 'abp.services.portal.role', 'abp.services.portal.roleForHost', 'tenantId',
            function ($scope, $uibModalInstance, $timeout, roleService, roleForHostService, tenantId) {
                var vm = this;

                vm.saving = false;

                //保存创建角色
                vm.save = function () {
                    abp.message.confirm('确定要创建新的角色吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.saving = true;
                            var servicePromise = null;
                            if (tenantId) {
                                //特定租户用户
                                servicePromise = roleForHostService.createRole(vm.role, tenantId);
                            } else {
                                //当前租户用户
                                servicePromise = roleService.createRole(vm.role);
                            }

                            servicePromise.success(function () {
                                abp.notify.success('角色创建成功！');
                                $uibModalInstance.close();
                            }).finally(function () {
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