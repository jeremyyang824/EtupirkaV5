(function () {
    'use strict';

    angular.module('app.main')
        .controller('rolesListController', [
            '$scope', '$rootScope', '$stateParams', 'abp.services.portal.tenant',
            function ($scope, $rootScope, $stateParams, tenantService) {

                $rootScope.setViewTitle('角色权限');

                var vm = this;
                vm.tenantId = $stateParams.tenantId || null;

                vm.selectedRole = null;
                vm.selectedPermissions = [];

                if (vm.tenantId) {
                    tenantService.getTenant(vm.tenantId)
                        .success(function (result) {
                            var tenant = result;
                            $rootScope.setViewTitle('角色权限', abp.utils.formatString('[{0}]{1}', tenant.tenancyName, tenant.name));  //设置页标题
                        });
                }

                //console.log(vm.tenantId);

            } //end controller
        ]);
})();