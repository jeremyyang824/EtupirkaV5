(function () {
    'use strict';

    angular.module('app.main')
        .factory('appWarehouseService', [
            '$q', 'abp.services.inventory.warehouse', 'abp.services.inventory.warehouseForHost',
            function ($q, warehouseService, warehouseForHostService) {

                //取得用户管理的仓库
                function getUserWarehouse() {
                    var deferred = $q.defer();
                    warehouseService.getUserWarehouses()
                        .success(function (data) {
                            deferred.resolve(data);
                        })
                        .error(function (resp) {
                            deferred.reject(resp);
                        });
                    return deferred.promise;
                }

                //取得租户的仓库
                function getTenantWarehouse(tenantId) {
                    var deferred = $q.defer();
                    warehouseForHostService.getWarehouses(tenantId, true)
                        .success(function (data) {
                            deferred.resolve(data);
                        })
                        .error(function (resp) {
                            deferred.reject(resp);
                        });
                    return deferred.promise;
                }

                return {
                    getUserWarehouse: getUserWarehouse,
                    getTenantWarehouse: getTenantWarehouse
                };
            }
        ]);
})();