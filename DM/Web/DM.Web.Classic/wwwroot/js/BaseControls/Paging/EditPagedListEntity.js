DM.EditPagedListEntityControl = Base.extend({
    constructor: function (options, view) {
        this._view = view || new DM.EditPagedListEntityControl.View(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("editRequestSuccess", this.handle("edited"), this);
        this._view.on("canceled", this.handle("canceled"), this);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._entityId = options.entityId;

            this._form = new DM.FormControl("#EditForm_" + this._entityId, {
                validate: true
            });
            this._input = this._form.$()
                .find("textarea")
                .last()
                .focus();
            this._cancelLink = $("#CancelEditLink_" + this._entityId);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;

            this._form.on("requestSuccess", this.handle("editRequestSuccess"), this);
            this._input
                .on("keyup.submit", function (evt) {
                    var btn = evt.keyCode || evt.which;
                    if (btn === 27) {
                        _this.trigger("canceled");
                    }
                });
            this._cancelLink.on("click.cancel", function () {
                _this.trigger("canceled");
            });
        }
    })
});