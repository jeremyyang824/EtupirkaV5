(function () {
    'use strict';

    angular.module('app.main')
        .controller('userCreateController', [
            '$scope', '$uibModalInstance', '$timeout', 'abp.services.portal.user', 'abp.services.portal.userForHost', 'tenantId',
            function ($scope, $uibModalInstance, $timeout, userService, userForHostService, tenantId) {
                var vm = this;

                vm.saving = false;

                //保存创建用户
                vm.save = function () {
                    abp.message.confirm('确定要创建新的用户吗？用户信息创建后不可删除！')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.saving = true;
                            var servicePromise = null;
                            if (tenantId) {
                                //特定租户用户
                                servicePromise = userForHostService.createUser(vm.user, tenantId);
                            } else {
                                //当前租户用户
                                servicePromise = userService.createUser(vm.user);
                            }

                            servicePromise.success(function () {
                                abp.notify.success('用户创建成功！');
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