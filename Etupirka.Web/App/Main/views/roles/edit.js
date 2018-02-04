(function () {
    'use strict';

    angular.module('app.main')
        .controller('roleEditController', [
            '$scope', '$uibModalInstance', '$timeout', 'abp.services.portal.role', 'abp.services.portal.roleForHost', 'roleId', 'tenantId',
            function ($scope, $uibModalInstance, $timeout, roleService, roleForHostService, roleId, tenantId) {
                var vm = this;

                vm.saving = false;
                vm.role = null;

                //保存创建角色
                vm.save = function () {
                    vm.saving = true;

                    var servicePromise = null;
                    if (tenantId) {
                        //特定租户用户
                        servicePromise = roleForHostService.updateRole(vm.role, tenantId);
                    } else {
                        //当前租户用户
                        servicePromise = roleService.updateRole(vm.role);
                    }

                    servicePromise.success(function () {
                        abp.notify.success('角色保存成功！');
                        $uibModalInstance.close();
                    }).finally(function () {
                        vm.saving = false;
                    });
                };

                vm.deleteRole = function () {
                    abp.message.confirm('确定要删除该角色吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            vm.saving = true;
                            var servicePromise = null;
                            if (tenantId) {
                                //特定租户用户
                                servicePromise = roleForHostService.deleteRole(vm.role.id, tenantId);
                            } else {
                                //当前租户用户
                                servicePromise = roleService.deleteRole(vm.role.id);
                            }

                            servicePromise.success(function () {
                                abp.notify.success('角色已删除！');
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
                        servicePromise = roleForHostService.getRole(roleId, tenantId);

                    } else {
                        //当前租户用户
                        servicePromise = roleService.getRole(roleId);
                    }

                    servicePromise.success(function (result) {
                        vm.role = result;
                    });
                }

                init();
            }
        ]);
})();