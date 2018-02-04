var abp = abp || {};
(function ($) {
    if (!sweetAlert || !$) {
        return;
    }

    /* DEFAULTS *************************************************/

    abp.libs = abp.libs || {};
    abp.libs.sweetAlert = {
        config: {
            'default': {

            },
            info: {
                type: 'info'
            },
            success: {
                type: 'success'
            },
            warn: {
                type: 'warning'
            },
            error: {
                type: 'error'
            },
            confirm: {
                type: 'warning',
                title: 'Are you sure?',
                showCancelButton: true,
                cancelButtonText: 'Cancel',
                confirmButtonColor: "#DD6B55",
                confirmButtonText: 'Yes'
            }
        }
    };

    /* MESSAGE **************************************************/

    var showMessage = function (type, message, title) {
        if (!title) {
            title = message;
            message = undefined;
        }

        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config.default,
            abp.libs.sweetAlert.config[type],
            {
                title: title,
                text: message
            }
        );

        //return $.Deferred(function ($dfd) {
        //    sweetAlert(opts, function () {
        //        $dfd.resolve();
        //    });
        //});

        //change to sweetalert2
        return sweetAlert(opts)
            .then(function (isConfirmed) {
                return new Promise(function (resolve, reject) {
                    resolve(isConfirmed);
                });
            }, function (dismiss) {
                if (dismiss === 'cancel') {
                    return false;
                } else {
                    throw dismiss;
                }
            });
    };

    abp.message.info = function (message, title) {
        return showMessage('info', message, title);
    };

    abp.message.success = function (message, title) {
        return showMessage('success', message, title);
    };

    abp.message.warn = function (message, title) {
        return showMessage('warn', message, title);
    };

    abp.message.error = function (message, title) {
        return showMessage('error', message, title);
    };

    //change to sweetalert2
    abp.message.confirm = function (message, title) {
        //if (!title) {
        //    title = message;
        //    message = undefined;
        //}

        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config.default,
            abp.libs.sweetAlert.config.confirm,
            {
                title: title,
                text: message
            }
        );

        //return $.Deferred(function ($dfd) {
        //    sweetAlert(opts, function (isConfirmed) {
        //        callback && callback(isConfirmed);
        //        $dfd.resolve(isConfirmed);
        //    });
        //});

        //change to sweetalert2
        return sweetAlert(opts)
            .then(function (isConfirmed) {
                return new Promise(function (resolve, reject) {
                    resolve(isConfirmed);
                });
            },
            function (dismiss) {
                if (dismiss === 'cancel') {
                    return false;
                } else {
                    throw dismiss;
                }
            });
    };

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        abp.libs.sweetAlert.config.confirm.title = abp.localization.abpWeb('AreYouSure');
        abp.libs.sweetAlert.config.confirm.cancelButtonText = abp.localization.abpWeb('Cancel');
        abp.libs.sweetAlert.config.confirm.confirmButtonText = abp.localization.abpWeb('Yes');
    });

})(jQuery);