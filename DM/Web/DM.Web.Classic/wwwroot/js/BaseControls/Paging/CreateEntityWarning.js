DM.CreateEntityWarningControl = DM.CreatePagedListEntityControl.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.CreateEntityWarningControl.View(options);
        this.base(options, this._view, proxy);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("createWarningSuccess", this.handle("create"), this);
    },
    show: function() {
        this._view.show();
    },
    hide: function() {
        this._view.hide();
    },
    toggle: function() {
        this._view.toggle();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._createWarningControl = $("#CreateWarningEntity_" + options.parentEntityId);
            this._warnForm = new DM.FormControl("#CreateWarningForm_" + options.parentEntityId, { validate: true });

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            this._warnForm
                .on("requestSuccess", this.handle("createWarningSuccess"), this)
                .on("requestSuccess", this._warnForm.reset, this._warnForm)
                .on("requestSuccess", this.hide, this);
        },
        show: function () {
            this._createWarningControl.show();
        },
        hide: function() {
            this._createWarningControl.hide();
        },
        toggle: function() {
            this._createWarningControl.toggle();
        }
    })
})