(function () {
    'use strict';

    $(function () {
        $('#LoginButton').click(function (e) {
            e.preventDefault();
            if ($("#LoginForm").valid()) {
                abp.ui.setBusy(
                    $('.login-box-body'),
                    abp.ajax({
                        url: abp.appPath + 'Account/Login',
                        type: 'POST',
                        data: JSON.stringify({
                            tenancyName: $('#TenancyName').val(),
                            username: $('#UserName').val(),
                            password: $('#Password').val(),
                            rememberMe: $('#RememberMe').is(':checked'),
                            returnUrlHash: $('#ReturnUrlHash').val()
                        })
                    })

                );
            }
        });

        $('#ReturnUrlHash').val(location.hash);

        $('input').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%' // optional
        });
    });

})();