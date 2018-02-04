(function () {
    'use strict';

    angular.module('app.main')
        .controller('layoutController', [
            '$scope', '$rootScope',
            function ($scope, $rootScope) {
                var vm = this;
                $rootScope.title = "主页";
                $rootScope.displayMode = abp.setting.getInt('Etupirka.DisplayLevel');

                /**
                 * 设置视图标题
                 * @param String 标题 
                 * @param String 子标题 
                 */
                $rootScope.setViewTitle = function (title, subTitle) {
                    title = (angular.isUndefined(title) || title == null) ? '' : title;
                    subTitle = (angular.isUndefined(subTitle) || subTitle == null) ? '' : subTitle;
                    $rootScope.title = title;
                    $rootScope.subTitle = subTitle;
                }
            }
        ]);

})();