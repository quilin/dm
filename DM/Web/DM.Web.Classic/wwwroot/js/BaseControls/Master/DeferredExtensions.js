(function ($) {
    var convertToPlainArray = function () {
        var result = [];

        for (var i = 0, l = arguments.length; i < l; i++) {
            var argument = arguments[i];
            if ($.isArray(argument)) {
                result = result.concat(convertToPlainArray.apply(this, argument));
            } else {
                result.push(argument);
            }
        }

        return result;
    };

    $.when.chain = function () {
        var dfd = $.Deferred();
        var promises = convertToPlainArray.apply(this, arguments);

        handlePromise(promises[0]);

        function handlePromise(promise, externalData) {
            if (promise === undefined) {
                dfd.resolve(externalData);
                return;
            }

            if (promise instanceof Function) {
                promise = promise();
            }

            promise
                .done(function (data) {
                    promises.shift();
                    handlePromise(promises[0], data);
                })
                .fail(function (data) {
                    dfd.reject(data);
                });
        }

        return dfd.promise();
    };
})(jQuery);