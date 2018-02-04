(function () {
    'use strict';

    angular.module('angular.ui.etupirka')
        .directive('iCheck', ['$timeout',
            function ($timeout) {
                return {
                    require: 'ngModel',
                    link: function ($scope, element, $attrs, ngModel) {
                        return $timeout(function () {
                            var value = $attrs['value'];
                            var $element = $(element);

                            var config = {
                                checkboxClass: 'icheckbox_square-blue',
                                radioClass: 'iradio_square-blue',
                                increaseArea: '20%'
                            };

                            //Get Configuration
                            var customConfig = $attrs['iCheck'];
                            if (customConfig) {
                                angular.extend(config, $scope.$eval(customConfig));
                            }

                            //Instantiate the iCheck control.    
                            $element.iCheck(config);

                            // If the model changes, update the iCheck control.
                            $scope.$watch($attrs['ngModel'], function (newValue) {
                                $element.iCheck('update');
                            });

                            // If the iCheck control changes, update the model.
                            $element.on('ifChanged', function (event) {
                                if ($element.attr('type') === 'checkbox' && $attrs['ngModel']) {
                                    $scope.$apply(function () {
                                        ngModel.$setViewValue(event.target.checked);
                                    });
                                }
                                if ($element.attr('type') === 'radio' && $attrs['ngModel']) {
                                    $scope.$apply(function () {
                                        ngModel.$setViewValue(value);
                                    });
                                }
                            });
                        });
                    } // end link
                };
            }
        ]);
})();