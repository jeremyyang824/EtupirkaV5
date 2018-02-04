(function () {
    'use strict';

    angular.module('app.main')
        .controller('userEditController', [
            '$scope', '$uibModalInstance', '$timeout', 'abp.services.portal.user', 'abp.services.portal.userForHost', 'userId', 'tenantId',
            function ($scope, $uibModalInstance, $timeout, userService, userForHostService, userId, tenantId) {
                var vm = this;

                vm.saving = false;
                vm.user = null;

                //保存用户信息修改
                vm.save = function () {
                    vm.saving = true;

                    var servicePromise = null;
                    if (tenantId) {
                        //特定租户用户
                        servicePromise = userForHostService.updateUser({
                            id: vm.user.id,
                            name: vm.user.name,
                            surname: vm.user.surname,
                            userName: vm.user.userName,
                            emailAddress: vm.user.emailAddress,
                            isActive: vm.user.isActive
                        }, tenantId);
                    } else {
                        //当前租户用户
                        servicePromise = userService.updateUser({
                            id: vm.user.id,
                            name: vm.user.name,
                            surname: vm.user.surname,
                            userName: vm.user.userName,
                            emailAddress: vm.user.emailAddress,
                            isActive: vm.user.isActive
                        });
                    }

                    servicePromise.success(function () {
                        abp.notify.success('用户保存成功！');
                        $uibModalInstance.close();
                    }).finally(function () {
                        vm.saving = false;
                    });
                };

                //重置密码
                vm.resetPassword = function () {
                    abp.message.confirm('确定要重置该用户吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.saving = true;
                            var servicePromise = null;
                            if (tenantId) {
                                //特定租户用户
                                servicePromise = userForHostService.resetPassword(vm.user.id, tenantId);

                            } else {
                                //当前租户用户
                                servicePromise = userService.resetPassword(vm.user.id);
                            }

                            servicePromise.success(function () {
                                abp.notify.success('密码重置成功！');
                                $uibModalInstance.close();
                            }).finally(function () {
                                vm.saving = false;
                            });
                        });
                };

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };


                function init() {

                    var servicePromise = null;
                    if (tenantId) {
                        //特定租户用户
                        servicePromise = userForHostService.getUser(userId, tenantId);

                    } else {
                        //当前租户用户
                        servicePromise = userService.getUser(userId);
                    }

                    servicePromise.success(function (result) {
                        vm.user = result;
                    });
                }

                init();
            }
        ]);
})();