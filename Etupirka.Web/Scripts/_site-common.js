var App = App || {};
(function () {
    'use strict';

    abp.localization.defaultSourceName = 'Etupirka';
    var appLocalizationSource = abp.localization.getSource('Etupirka');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

    //重写toJSON，解决AJAX请求转JSON时，日期丢失时区的问题
    Date.prototype.toJSON = function() {
        return this.toLocaleString();
    }

    //--consts----------------------------------------------
    App.consts = {
        hostInfo: {
            fullName: '上海烟草机械有限公司',
            name: '上海烟草机械'
        },
        userManagement: {
            defaultAdminUserName: 'admin'
        },
        grid: {
            defaultPageSize: 10,
            defaultPageSizes: [10, 20, 50, 100]
        },
        contentTypes: {
            formUrlencoded: 'application/x-www-form-urlencoded; charset=UTF-8'
        }
    };


    //--AdminLTE--------------------------------------------
    App.AdminLTE = (function ($, AdminLTE) {

        var _skins = [
            "skin-blue",
            "skin-black",
            "skin-red",
            "skin-yellow",
            "skin-purple",
            "skin-green",
            "skin-blue-light",
            "skin-black-light",
            "skin-red-light",
            "skin-yellow-light",
            "skin-purple-light",
            "skin-green-light"
        ];

        return {
            /**
             * 改变AdminLTE布局类型（包括控制边栏）
             * @param String sidebar-collapse; fixed; layout-boxed; 
             */
            changeLayout: function (cls) {
                console.log('change layout type to :' + cls);

                $("body").toggleClass(cls);
                AdminLTE.layout.fixSidebar();

                //Fix the problem with right sidebar and layout boxed
                if ($(".control-sidebar") && $(".control-sidebar-bg")) {
                    if (cls === "layout-boxed")
                        AdminLTE.controlSidebar._fix($(".control-sidebar-bg"));
                    if ($('body').hasClass('fixed') && cls === 'fixed') {
                        AdminLTE.pushMenu.expandOnHover();
                        AdminLTE.layout.activate();
                    }
                    AdminLTE.controlSidebar._fix($(".control-sidebar-bg"));
                    AdminLTE.controlSidebar._fix($(".control-sidebar"));
                }
            },
            /**
             * 改变AdminLTE皮肤
             * @param 皮肤class
             */
            changeSkin: function (cls) {
                $.each(_skins, function (i) {
                    $("body").removeClass(_skins[i]);
                });

                $("body").addClass(cls);
                store('skin', cls);
                return false;
            }
        };

        function store(name, val) {
            if (typeof (Storage) !== "undefined") {
                localStorage.setItem(name, val);
            } else {
                window.alert('Please use a modern browser to properly view this template!');
            }
        }

        function get(name) {
            if (typeof (Storage) !== "undefined") {
                return localStorage.getItem(name);
            } else {
                window.alert('Please use a modern browser to properly view this template!');
            }
        }

    })(jQuery, $.AdminLTE);

    //--end AdminLTE----------------------------------------

    //--utility---------------------------------------------
    App.util = {
        /**
         * 将字符串解析为boolean，转换失败返回null
         * @returns bool? 
         */
        parseBool: function (val) {
            if (val === null || val === undefined)
                return null;
            if (typeof val === 'boolean')
                return val;

            val = val.toString();

            if (val === '1' || val === 'true')
                return true;
            else if (val === '0' || val === 'false')
                return false;
            else
                return null;
        }
    };

    //--end utility---------------------------------------------

    //--jquery.validate--------------------------------------------
    jQuery.validator.setDefaults({
        highlight: function (element, errorClass, validClass) {
            var $container = $(element).parent('.has-feedback');
            if ($container.length > 0) {
                $container.addClass('has-error');
            }
        },
        unhighlight: function (element, errorClass, validClass) {
            var $container = $(element).parent('.has-feedback');
            if ($container.length > 0) {
                $container.removeClass('has-error');
            }
        }
    });
    //--end jquery.validate----------------------------------------

})(App);