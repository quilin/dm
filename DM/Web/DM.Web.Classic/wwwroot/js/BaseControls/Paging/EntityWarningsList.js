DM.EntityWarningsListControl = Base.extend({
    constructor: function(options, view, proxy, warningFactory, warningFormFactory) {
        this._view = view || new DM.EntityWarningsListControl.View(options);
        this._proxy = proxy || new DM.EntityWarningsListControl.Proxy(options);
        this._warningFactory = warningFactory || new DM.EntityWarningsListControl.WarningControlFactory(options);
        this._warningFormFactory = warningFormFactory || new DM.EntityWarningsListControl.CreateWarningFormFactory(options);

        this._proxy.setWarningsListUrl(this._view.getWarningsListUrl());

        this._warnings = {};

        this._initWarnings();
        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("getWarningFormRequest", this._proxy.getWarningForm, this._proxy);
        this._proxy.on("getWarningFormRequestBegin", this._view.getWarningFormRequestBegin, this._view);
        this._proxy.on("getWarningFormRequestComplete", this._view.getWarningFormRequestComplete, this._view);
        this._proxy.on("getWarningFormRequestSuccess", this._view.getWarningFormRequestSuccess, this._view);

        this._view.on("createWarningFormRendered", this._initializeWarningForm, this);
        this._view.on("toggleWarnForm", this._toggleWarnForm, this);

        this._proxy.on("getWarningsListRequestBegin", this._view.getWarningsListRequestBegin, this._view);
        this._proxy.on("getWarningsListRequestComplete", this._view.getWarningsListRequestComplete, this._view);
        this._proxy.on("getWarningsListRequestSuccess", this._getWarningsListRequestSuccess, this);
    },
    cancelWarnings: function() {
        this._resolveEditing();
    },
    _initializeWarningForm: function() {
        this._warnForm = this._warningFormFactory.create();
        this._warnForm.on("created", this._processCreatedWarning, this);
        this._warnForm.show();
    },
    _toggleWarnForm: function() {
        this._warnForm.toggle();
    },
    _cancelWarn: function() {
        if (this._warnForm) {
            this._warnForm.hide();
        }
    },
    _getWarningsListRequestSuccess: function (data) {
        this._view.getWarningsListRequestSuccess(data);
        this._initWarnings();
    },
    _initWarnings: function() {
        var warningIds = this._view.getWarningIds();
        for (var i = 0; i < warningIds.length; ++i) {
            this._initWarning(warningIds[i]);
        }
    },
    _initWarning: function(warningId) {
        var warning = this._warnings[warningId] = this._warningFactory.create(warningId);
        warning.on("removed", this._processRemovedWarning, this);
        warning.on("edited", this._processEditedWarning, this);
        warning.on("editInitiated", this._resolveEditing, this);
    },
    _resolveEditing: function (editedWarningId) {
        this._cancelWarn();
        for (var warningId in this._warnings) {
            if (warningId !== editedWarningId) {
                this._warnings[warningId].cancelEdit();
            }
        }
    },
    _processEditedWarning: function() {
        this._cancelWarn();
        this.trigger("editedWarning");
        this._initWarnings();
    },
    _processCreatedWarning: function () {
        this._cancelWarn();
        this.trigger("createdWarning");
        this._proxy.getWarningsList();
    },
    _processRemovedWarning: function() {
        this._cancelWarn();
        this.trigger("removedWarning");
        this._proxy.getWarningsList();
    }
},
{
    View: Base.extend({
        constructor: function(options) {
            this._entityId = options.entityId;

            // Warnings
            this._warnLink = $("#WarnEntityLink_" + this._entityId);
            this._warnLinkLoader = DM.Loader.create(this._warnLink);
            this._warningsBlock = $("#WarningsEntity_" + this._entityId);
            this._warningsBlockLoader = DM.Loader.create(this._warningsBlock);
            this._createWarningBlock = $("#CreateWarningEntity_" + this._entityId);

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._warnLink.on("click.request",
                function(evt) {
                    evt.preventDefault();
                    _this.trigger("getWarningFormRequest", this.href);
                });
        },

        getWarningsListUrl: function() {
            return this._warningsBlock.data("warnings-list-url");
        },

        // Warn form
        getWarningFormRequestBegin: function() {
            this._warnLinkLoader.show();
        },
        getWarningFormRequestComplete: function() {
            this._warnLinkLoader.hide();
        },
        getWarningFormRequestSuccess: function(data) {
            var _this = this;
            var form = this._createWarningBlock.html(data);
            this._warnLink
                .off("click.request")
                .on("click.toggleForm",
                    function(evt) {
                        evt.preventDefault();
                        _this.trigger("toggleWarnForm");
                    });
            this.trigger("createWarningFormRendered");
        },
        // end Warn form

        // Warnings list
        getWarningsListRequestBegin: function() {
            this._warningsBlockLoader.show();
        },
        getWarningsListRequestComplete: function() {
            this._warningsBlockLoader.hide();
        },
        getWarningsListRequestSuccess: function (data) {
            this._warningsBlock.html(data);
        },
        getWarningIds: function(data) {
            var blocks = data ? $(data) : this._warningsBlock.find(".warning");
            return blocks.map(function() {
                return this.getAttribute("data-entity-id");
            });
        }
        // end Warnings list
    }),
    Proxy: Base.extend({
        constructor: function(options) {
        },
        setWarningsListUrl: function(url) {
            this._warningsListUrl = url;
        },
        getWarningForm: function(url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getWarningFormRequestBegin"),
                complete: this.handle("getWarningFormRequestComplete"),
                success: this.handle("getWarningFormRequestSuccess")
            });
        },
        getWarningsList: function() {
            $.ajax({
                type: "GET",
                url: this._warningsListUrl,
                context: this,
                beforeSend: this.handle("getWarningsListRequestBegin"),
                complete: this.handle("getWarningsListRequestComplete"),
                success: this.handle("getWarningsListRequestSuccess")
            });
        }
    }),
    WarningControlFactory: Base.extend({
        create: function(warningId) {
            return new DM.EntityWarningControl({
                warningId: warningId
            });
        }
    }),
    CreateWarningFormFactory: Base.extend({
        constructor: function(options) {
            this._entityId = options.entityId;
        },
        create: function () {
            return new DM.CreateEntityWarningControl({
                parentEntityId: this._entityId
            });
        }
    })
});