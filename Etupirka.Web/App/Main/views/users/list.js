(function () {
    'use strict';

    angular.module('app.main')
        .controller('usersListController', [
            '$scope', '$rootScope', '$stateParams', 'abp.services.portal.tenant',
            function ($scope, $rootScope, $stateParams, tenantService) {

                $rootScope.setViewTitle('用户角色');

                var vm = this;
                vm.tenantId = $stateParams.tenantId || null;
                vm.filterText = $stateParams.filterText || null;
                vm.filterIsActive = App.util.parseBool($stateParams.filterIsActive);
                vm.selectedUser = null;
                vm.selectedRoles = [];

                if (vm.tenantId) {
                    tenantService.getTenant(vm.tenantId)
                        .success(function (result) {
                            var tenant = result;
                            $rootScope.setViewTitle('用户角色', abp.utils.formatString('[{0}]{1}', tenant.tenancyName, tenant.name));  //设置页标题
                        });
                }

                //console.log(vm.tenantId);

                vm.getSelectedRoles = function () {
                    return _.map(vm.selectedRoles, function (role) {
                        return role.name;
                    });
                };

            } //end controller
        ]);
})();