﻿var _getCookieValue = function (key) {
    var equalities = document.cookie.split(';');
    for (var i = 0; i < equalities.length; i++) {
        if (!equalities[i]) {
            continue;
        }

        var splitted = equalities[i].split('=');
        if (splitted.length !== 2) {
            continue;
        }

        if (decodeURIComponent(splitted[0]) === key) {
            return decodeURIComponent(splitted[1] || '');
        }
    }
    return null;
};
var _csrfCookie = _getCookieValue("XSRF-TOKEN");
var _csrfCookieAuth = new SwaggerClient.ApiKeyAuthorization("X-XSRF-TOKEN", _csrfCookie, "header");
swaggerUi.api.clientAuthorizations.add("X-XSRF-TOKEN", _csrfCookieAuth);