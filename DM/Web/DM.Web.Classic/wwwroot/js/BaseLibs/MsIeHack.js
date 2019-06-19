(function () {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (!!~msie || !!ua.match(/Trident.*rv\:11\./)) {
        $.ajaxSetup({ cache: false });
    }
})();