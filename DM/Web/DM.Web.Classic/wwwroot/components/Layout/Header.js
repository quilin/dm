DM.Header = Base.extend({
    constructor: function (options, view, proxy, loginControlFactory, registerControlFactory) {
        this._view = view || new DM.Header.View(options);
        this._proxy = proxy || new DM.Header.Proxy(options);
        this._loginControlFactory = loginControlFactory || new DM.Header.LoginControlFactory(options);
        this._registerControlFactory = registerControlFactory || new DM.Header.RegisterControlFactory(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("getLoginFormRequest", this._getLoginForm, this);
        this._proxy.on("getLoginFormRequestBegin", this._view.getLoginFormRequestBegin, this._view);
        this._proxy.on("getLoginFormRequestComplete", this._view.getLoginFormRequestComplete, this._view);
        this._proxy.on("getLoginFormRequestSuccess", this._getLoginFormRequestSuccess, this);

        this._view.on("getRegisterFormRequest", this._getRegisterForm, this);
        this._proxy.on("getRegisterFormRequestBegin", this._view.getRegisterFormRequestBegin, this._view);
        this._proxy.on("getRegisterFormRequestComplete", this._view.getRegisterFormRequestComplete, this._view);
        this._proxy.on("getRegisterFormRequestSuccess", this._getRegisterFormRequestSuccess, this);
    },
    _getLoginForm: function (url) {
        if (!this._loginControl) {
            this._proxy.getLoginForm(url);
        }
    },
    _getLoginFormRequestSuccess: function (data) {
        this._loginControl = this._loginControlFactory.create(data);
    },
    _getRegisterForm: function (url) {
        if (!this._registerControl) {
            this._proxy.getRegisterForm(url);
        }
    },
    _getRegisterFormRequestSuccess: function (data) {
        this._registerControl = this._registerControlFactory.create(data);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._loginLink = $("#LoginLink");
            this._loginLoader = DM.Loader.create(this._loginLink);

            this._registerLink = $("#RegisterLink");
            this._registerLoader = DM.Loader.create(this._registerLink);

            this._logoutLink = $("#LogoutLink");
            
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._loginLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("getLoginFormRequest", this.href);
            });
            this._registerLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("getRegisterFormRequest", this.href);
            });
            this._logoutLink.on("click.logout", function (evt) {
                evt.preventDefault();
                var logoutUrl = this.href;
                DM.Confirm("Выход с сайта", {
                    title: "Уже уходите?",
                    confirmText: "Выйти"
                }).done(function () {
                    document.location.href = logoutUrl;
                });
            })
        },
        getLoginFormRequestBegin: function () {
            this._loginLoader.show();
        },
        getLoginFormRequestComplete: function () {
            this._loginLoader.hide();
        },
        getRegisterFormRequestBegin: function () {
            this._registerLoader.show();
        },
        getRegisterFormRequestComplete: function () {
            this._registerLoader.hide();
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
        },
        getLoginForm: function (url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getLoginFormRequestBegin"),
                complete: this.handle("getLoginFormRequestComplete"),
                success: this.handle("getLoginFormRequestSuccess")
            });
        },
        getRegisterForm: function (url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getRegisterFormRequestBegin"),
                complete: this.handle("getRegisterFormRequestComplete"),
                success: this.handle("getRegisterFormRequestSuccess")
            });
        }
    }),
    LoginControlFactory: Base.extend({
        create: function (data) {
            return new DM.LoginControl(data);
        }
    }),
    RegisterControlFactory: Base.extend({
        create: function (data) {
            return new DM.RegisterControl(data);
        }
    })
});