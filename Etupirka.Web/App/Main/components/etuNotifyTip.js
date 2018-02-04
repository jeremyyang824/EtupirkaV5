(function () {
    'use strict';

    angular.module('angular.ui.etupirka')
        .controller('etuNotifyTipController', [
            '$rootScope',
            function($rootScope) {
                var vm = this;
                vm.count = vm.count || 0;
                vm.color = vm.color || 'olive';
                vm.href = vm.href || '#';
                vm.icon = vm.icon || 'ion ion-stats-bars';
            }
        ])
        .directive('etuNotifyTipComponent', function() {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    title: '@',
                    count: '@',
                    color: '@',
                    href: '@',
                    icon: '@'
                },
                controller: 'etuNotifyTipController',
                controllerAs: 'vm',
                bindToController: true,
                templateUrl: '~/App/Main/components/etuNotifyTip.cshtml'
            };
        });
})();