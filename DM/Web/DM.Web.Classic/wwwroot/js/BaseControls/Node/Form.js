DM.FormControl = Base.extend({
    constructor: function (form, options, view, proxy) {
        this._view = view || new DM.FormControl.View(form, options);
        this._proxy = proxy || new DM.FormControl.Proxy();

        this._isRequesting = false;
        this._updateDefaultDataAfterSuccess = options && options.updateDefaultDataAfterSuccess;
        this._validateAtStart = options && options.validate;

        if (this._validateAtStart) {
            this.validate();
        }

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("requestValidation", this.handle("requestValidation"), this);
        this._view.on("toggledEnable", this.handle("toggledEnable"), this);
        this._view.on("request", this._request, this);
        this._proxy.on("requestBegin", this._requestBegin, this);
        this._proxy.on("requestComplete", this._requestComplete, this);
        this._proxy.on("requestError", this._view.requestError, this._view);
        this._proxy.on("requestSuccess", this._requestSuccess, this);
    },

    validate: function () {
        this._view.validate();
    },
    isValid: function() {
        return this._view.isValid();
    },
    submit: function () {
        this._view.submit();
    },
    reset: function () {
        this._view.reset();
    },
    serialize: function() {
        return this.$().serialize();
    },

    resolveEnabled: function() {
        this._view.resolveEnabled();
    },
    disable: function () {
        this._view.toggleEnable(false);
    },
    enable: function () {
        this._view.toggleEnable(true);
    },

    $: function () {
        return this._view.$();
    },

    interfere: function(callback) {
        this._interfereCallback = callback;
    },

    _request: function (url, type, data) {
        if (!this._isRequesting) {
            if (this._interfereCallback) {
                data = this._interfereCallback(data);
            }
            this._proxy.request(url, type, data);
        }
    },
    _requestBegin: function () {
        this._isRequesting = true;
        this._view.requestBegin();
        this.trigger("requestBegin");
    },
    _requestComplete: function () {
        this._isRequesting = false;
        this._view.requestComplete();
    },
    _requestSuccess: function (data) {
        this.trigger("requestSuccess", data);
        this._view.requestSuccess();
        if (this._updateDefaultDataAfterSuccess) {
            this._view.updateDefaultData();
        }
    }
}, {
    View: Base.extend({
        constructor: function (form, options) {
            this._form = $(form);
            this._submit = this._form.find("[type='submit']");
            this._loader = DM.Loader.create(this._form);
            this._nonAjax = options && options.nonAjax;
            this._keepLoader = options && options.keepLoader;

            this._isDisabled = undefined;

            this.updateDefaultData();
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._form
                .on("submit.request", function (evt) {
                    if (!_this._nonAjax) {
                        evt.preventDefault();
                    }
                    if (!_this._submit.prop("disabled")) {
                        _this.trigger("requestValidation");
                        var formIsValid = _this.isValid();
                        if (formIsValid && !_this._nonAjax) {
                            _this.trigger("request", this.action, this.method, _this._form.serialize());
                        } else if (formIsValid) {
                            _this.requestBegin();
                        }
                    }
                })
                .on("input change", function (evt) { _this.resolveEnabled(); })
                .on("input.valueChanged", "input, textarea", function (evt) { _this.resolveEnabled(); });
        },

        validate: function () {
            $.validator.unobtrusive.parse(this._form);
        },
        isValid: function() {
            return this._form.valid();
        },
        submit: function () {
            this._form.submit();
        },
        updateDefaultData: function () {
            this._defaultFormData = this.__generateFormHash();
            this.resolveEnabled();
        },
        reset: function () {
            this._form.resetForm();
        },

        toggleEnable: function (enable) {
            var toDisable = !enable;
            if (toDisable !== this._isDisabled) {
                this._submit.prop("disabled", toDisable);
                this._isDisabled = toDisable;
                this.trigger("toggledEnable", enable);
            }
        },

        resolveEnabled: function () {
            var formData = this.__generateFormHash();
            this.toggleEnable(formData !== this._defaultFormData);
        },
        __generateFormHash: function () {
            return this._form.serialize();
        },

        $: function () {
            return this._form;
        },

        requestBegin: function () {
            this._loader.show();
        },
        requestComplete: function () {
            this._loader.hide();
        },
        requestSuccess: function() {
            if (!this._keepLoader) {
                this._loader.show();
            }
        },
        requestError: function (xhr) {
            try {
                var errorData = $.parseJSON(xhr.responseText).obj;
                var validator = this._form.validate();
                this._form.find(".field-validation-error").text('');
                validator.showErrors(errorData);
                this._form.find("input.input-validation-error").first().focus();
            } catch (e) {}
        }
    }),
    Proxy: Base.extend({
        request: function (url, type, data) {
            $.ajax({
                url: url,
                type: type,
                data: data,
                context: this,
                beforeSend: this.handle("requestBegin"),
                complete: this.handle("requestComplete"),
                error: this.handle("requestError"),
                success: this.handle("requestSuccess")
            });
        }
    })
});