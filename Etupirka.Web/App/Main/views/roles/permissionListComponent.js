(function () {
    'use strict';

    angular.module('app.main')
        .controller('permissionListComponentController', [
            '$scope', '$uibModal', '$q', '$timeout',
            'abp.services.portal.permission', 'abp.services.portal.permissionForHost',
            function ($scope, $uibModal, $q, $timeout, permissionService, permissionForHostService) {

                var vm = this;
                vm.permissionLoading = false;
                vm.permissionTreeData = null;   //权限资源树数据源

                //绑定权限资源（并绑定角色被授予的权限）
                vm.bindPermissions = function () {
                    vm.permissionLoading = true;

                    var permissionServicePromise = null;    //取得所有权限资源
                    if (vm.tenantId) {
                        //特定租户用户
                        permissionServicePromise = permissionForHostService.getAllPermissions(vm.tenantId);
                    } else {
                        //当前租户用户
                        permissionServicePromise = permissionService.getAllPermissions();
                    }

                    permissionServicePromise.success(function (data) {
                        vm.permissionTreeData = {
                            permissions: data.items,      //所有权限资源
                            grantedPermissionNames: null  //角色拥有的权限
                        };
                    }).finally(function () {
                        vm.permissionLoading = false;
                    });

                }

                //监视selectedPermissions
                $scope.$watch("vm.selectedPermissions",
                    function (newVal, oldVal) {
                        if (newVal === oldVal)
                            return;

                        if (vm.permissionTreeData) {
                            vm.permissionTreeData = {
                                permissions: vm.permissionTreeData.permissions, //所有权限资源
                                grantedPermissionNames: newVal                  //角色拥有的权限
                            };
                        }
                    });

                //监视勾选的权限(同步到参数vm.selectedPermissions)
                $scope.$watch("vm.permissionTreeData.grantedPermissionNames",
                    function (newVal, oldVal) {
                        if (newVal === oldVal)
                            return;

                        $timeout(function () {
                            if (!vm.selectedPermissions)
                                vm.selectedPermissions = [];

                            vm.selectedPermissions.length = 0;
                            if (vm.permissionTreeData && vm.permissionTreeData.grantedPermissionNames) {
                                angular.forEach(vm.permissionTreeData.grantedPermissionNames, function (permissionName) {
                                    vm.selectedPermissions.push(permissionName);
                                });
                            }
                        });
                    });

                vm.bindPermissions();
            }
        ])
        .directive('permissionListComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    tenantId: '@',
                    selectedPermissions: '='   //选择的角色数组
                },
                controller: 'permissionListComponentController',
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '~/App/Main/views/roles/permissionListComponent.cshtml'
            };
        });

})();