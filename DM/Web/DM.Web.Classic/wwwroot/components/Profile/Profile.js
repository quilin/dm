DM.ProfileControl = Base.extend({
    constructor: function (options, view, proxy, personalDataFieldControlFactory, profilePictureUploadControl) {
        this._view = view || new DM.ProfileControl.View(options);
        this._proxy = proxy || new DM.ProfileControl.Proxy(options);
        this._profilePictureUploadControl = profilePictureUploadControl || new DM.PictureUploadControl({suffix: "profile"});

        this._personalDataFieldControlFactory = personalDataFieldControlFactory || new DM.ProfileControl.PersonalDataFieldControlFactory();

        this._personalDataFieldControls = { };
        this._initPersonalDataFieldControls();

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("closeSessionsRequest", this._proxy.closeSessionsRequest, this._proxy);
        this._proxy.on("closeSessionsBegin", this._view.closeSessionsBegin, this._view);
        this._proxy.on("closeSessionsComplete", this._view.closeSessionsComplete, this._view);
        this._proxy.on("closeSessionsSuccess", this._view.closeSessionsSuccess, this._view);
    },
    _initPersonalDataFieldControls: function () {
        var _this = this,
            types = this._view.getDataFieldTypes();
        for (var i = 0; i < types.length; ++i) {
            var type = types[i];
            this._personalDataFieldControls[type] = this._personalDataFieldControlFactory.create(type);
            this._personalDataFieldControls[type].on("editing", function() {
                for (var t in _this._personalDataFieldControls) {
                    _this._personalDataFieldControls[t].cancel();
                }
            });
        }
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._tabs = new DM.TabsControl(options);
            this._editInfoForm = new DM.FormControl("#ProfileInfoForm", {
                updateDefaultDataAfterSuccess: true
            });

            this._reportUserLink = $("#ReportUserLink");
            this._reportUserLightbox = DM.Lightbox.create("#ReportUserLightbox", {
                openLink: this._reportUserLink
            });
            this._reportUserForm = new DM.FormControl("#ReportUserForm");
            this._reportUserSuccessMessage = $("#ReportUserSuccessMessage");

            this._editInfoLink = $("#EditProfileInfoLink");
            this._cancelEditInfoLink = $("#CancelEditProfileInfoLink");
            this._info = $("#ProfileInfo");

            this._closeSessionsLink = $("#CloseSessionsLink");
            this._closeSessionsSuccessMessage = $("#CloseSessionsSuccessMessage");
            this._closeSessionsLoader = DM.Loader.create(this._closeSessionsLink);

            var editEmailLink = $("#ChangeEmailLink");
            if (editEmailLink.length > 0) {
                this._editEmailLightbox = new DM.FormLightboxControl({ link: editEmailLink });
                this._editEmailSuccessMessage = $("#EmailChangeSuccessMessage");
            }
            var editPasswordLink = $("#ChangePasswordLink");
            if (editPasswordLink.length > 0) {
                this._editPasswordLightbox = new DM.FormLightboxControl({ link: editPasswordLink });
                this._editPasswordSuccessMessage = $("#PasswordChangeSuccessMessage");
            }

            var initiateMergeLink = $("#InitiateMergeLink");
            if (initiateMergeLink.length > 0) {
                this._initiateMergeLightbox = new DM.FormLightboxControl({ link: initiateMergeLink });
                this._initiateMergeSuccessMessage = $("#InitiateMergeSuccessMessage");
            }
            var completeMergeLink = $("#CompleteMergeLink");
            if (completeMergeLink.length > 0) {
                this._completeMergeLightbox = new DM.FormLightboxControl({ link: completeMergeLink });
                this._completeMergeSuccessMessage = $("#CompleteMergeSuccessMessage");
            }

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._editInfoLink.on("click.showForm", function() {
                _this._toggleForm(true);
            });
            this._cancelEditInfoLink.on("click.hideForm", function() {
                _this._toggleForm(false);
            });
            this._editInfoForm.on("requestSuccess", this._updateInfo, this);
            this._reportUserForm.on("requestSuccess", this._reportUserSuccess, this);

            this._closeSessionsLink.on("click", function(evt) {
                evt.preventDefault();
                _this.trigger("closeSessionsRequest", this.href);
            });

            this._tabs.on("tabOpened", function(tabId) {
                if (tabId === "Settings" && !this._settingsActivated) {
                    var settingsForm = new DM.FormControl($("#UserSettingsForm"), {
                        validate: true,
                        updateDefaultDataAfterSuccess: true
                    });
                    var $dropdown = $(".js-colorschema-dropdown");
                    $dropdown.removeClass("js-colorschema-dropdown");
                    var colorSchemaDropdown = $dropdown.dropdown(true);
                    colorSchemaDropdown.on("select", function (option) {
                        $("html").attr("class", "colorschema_" + option.data("value").toLowerCase());
                    });
                }
            });

            if (this._editEmailLightbox) {
                this._editEmailLightbox.on("requestSuccess", function() {
                    this._editEmailSuccessMessage.slideToggleTimeout();
                }, this);
            }
            if (this._editPasswordLightbox) {
                this._editPasswordLightbox.on("requestSuccess", function () {
                    this._editPasswordSuccessMessage.slideToggleTimeout();
                }, this);
            }
            if (this._initiateMergeLightbox) {
                this._initiateMergeLightbox.on("requestSuccess", function () {
                    DM.Alert("Вы начали процесс слияния профилей!<br />Чтобы его завершить, нужно перелогиниться под профилем, с которым вы начали слияние, и нажать на кнопку \"Объединить профили\" на странице профиля.");
                    this._initiateMergeSuccessMessage.slideToggleTimeout();
                }, this);
            }
            if (this._completeMergeLightbox) {
                this._completeMergeLightbox.on("requestSuccess", function () {
                    this._completeMergeSuccessMessage.slideToggleTimeout();
                }, this);
            }
        },
        _toggleForm: function(show) {
            this._editInfoForm.$().toggle(show);
            this._info.toggle(!show);
            this._editInfoLink.toggle(!show);
        },
        _updateInfo: function(info) {
            this._info.html(info);
            this._toggleForm(false);
        },
        _reportUserSuccess: function(data) {
            this._reportUserLightbox.close();
            this._reportUserSuccessMessage.show();

            if (!data.canReport) {
                this._reportUserLightbox.remove();
                this._reportUserLink.remove();
            }
        },
        getDataFieldTypes: function() {
            return $(".js-profile-personal-data-type").map(function() {
                return this.getAttribute("data-type");
            });
        },
        closeSessionsBegin: function() {
            this._closeSessionsLoader.show();
        },
        closeSessionsComplete: function () {
            this._closeSessionsLoader.hide();
        },
        closeSessionsSuccess: function () {
            this._closeSessionsSuccessMessage.slideToggleTimeout();
        }
    }),
    PersonalDataFieldControlFactory: Base.extend({
        create: function (type) {
            return new DM.PersonalDataFieldControl(type);
        }
    }),
    Proxy: Base.extend({
        closeSessionsRequest: function (url) {
            $.ajax({
                url: url,
                context: this,
                beforeSend: this.handle("closeSessionsBegin"),
                complete: this.handle("closeSessionsComplete"),
                success: this.handle("closeSessionsSuccess")
            });
        }
    })
});