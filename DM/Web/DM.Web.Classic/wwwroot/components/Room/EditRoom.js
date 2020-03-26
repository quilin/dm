DM.EditRoomControl = Base.extend({
    constructor: function (options, view) {
        this._view = view || new DM.EditRoomControl.View(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("edited", this.handle("roomUpdated"), this);
        this._view.on("cancel", this.handle("cancel"), this);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._roomId = options.roomId;

            this._form = new DM.FormControl("#EditRoomForm_" + this._roomId, {
                updateDefaultDataAfterSuccess: true,
                validate: true
            });
            this._cancelLink = $("#CancelEditLink_" + this._roomId);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._form.on("requestSuccess", this.handle("edited"), this);
            this._cancelLink.on("click.cancelForm", function () {
                _this.trigger("cancel");
                _this._form.reset();
            });
        }
    })
});