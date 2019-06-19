DM.NotInformerControl = (function() {
    var NotInformerControl = Base.extend({
        constructor: function(options, view) {
            this._view = view || new NotInformerControl.View(options);
        },
        show: function(message) {
            this._view.show(message, false);
        },
        error: function (message) {
            this._view.show(message, true);
        }
    }, {
        View: Base.extend({
            constructor: function(options) {
                this._informer = $("#Informer");

                this.__attachEventListeners();
            },
            __attachEventListeners: function() {
                var _this = this;
                $(document)
                    .ajaxStart(function() {
                        if (_this._longAjaxTimeout) {
                            clearTimeout(_this._longAjaxTimeout);
                        }

                        _this._longAjaxTimeout = setTimeout(function() {
                            _this.show("<b>Пожалуйста, подождите</b><br>Идет обработка запроса");
                            _this._longAjaxTimeout = null;
                        }, 2000);
                    })
                    .ajaxStop(function() { _this._clearAjaxTimeout(); })
                    .ajaxError(function(evt, xhr) {
                        _this._clearAjaxTimeout();

                        var errorNumber = xhr.status % 100;
                        var defaultErrorMessage = "<b>Произошла ужасная ошибка!</b><br>Но мы о ней уже знаем. Может быть, попробуете еще раз?";

                        if (errorNumber === 5) { // 500+ error
                            _this.showError(defaultErrorMessage);
                        } else if (errorNumber === 4) {
                            switch (xhr.status) {
                                case 401:
                                    _this.showError("<b>Ошибка!</b>Видимо, у вас закончилась сессия. Попробуйте обновить страницу");
                                case 403:
                                    _this.showError("<b>Атата!</b>Вам нельзя это делать.");
                                default:
                                    _this.showError(defaultErrorMessage);
                            }
                        }
                    });
            },

            _clearAjaxTimeout: function () {
                if (this._longAjaxTimeout) {
                    clearTimeout(this._longAjaxTimeout);
                    this._longAjaxTimeout = null;
                }
            },
            _clearShowTimeout: function () {
                if (this._showTimeout) {
                    clearTimeout(this._showTimeout);
                    this._showTimeout = null;
                }
            },
            showError: function (message) { this.show(message, true); },
            show: function (message, isError) {
                this._informer
                    .toggleClass("informer-error", isError)
                    .html(message);

                this._clearShowTimeout();

                this._informer.show();

                var _this = this;
                this._showTimeout = setTimeout(function() {
                    _this.hide();
                    _this._showTimeout = null;
                }, 8000);
            },
            hide: function() {
                this._clearShowTimeout();
                this._informer.hide();
            }
        })
    });
    return new NotInformerControl();
})();