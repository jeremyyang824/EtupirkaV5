(function () {
    'use strict';

    angular.module('app.main')
        /**
         * 获取当前会话（租户、用户）
         */
        .factory('appSessionService', [
            function () {
                var session = {
                    isMultiTenancy: abp.multiTenancy.isEnabled,
                    currentUser: null,
                    currentTenant: null
                };

                abp.services.portal.session.getCurrentLoginInformations({ async: false })
                    .done(function (result) {
                        session.currentUser = result.user;
                        session.currentTenant = result.tenant;
                        session.organizationUnit = result.organizationUnit;
                    });

                return session;
            }
        ]);
})();