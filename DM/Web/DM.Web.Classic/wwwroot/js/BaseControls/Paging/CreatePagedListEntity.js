DM.CreatePagedListEntityControl = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.CreatePagedListEntityControl.View(options);
        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("createRequestSuccess", this.handle("created"), this);
    },
    toggle: function(show) {
        this._view.toggle(show);
    }
},
{
    View: Base.extend({
        constructor: function(options) {
            this._form = new DM.FormControl("#CreateForm", { validate: true });
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            this._form
                .on("requestSuccess", this.handle("createRequestSuccess"), this)
                .on("requestSuccess", this._form.reset, this._form);
        },
        toggle: function(show) {
            this._form.$().toggle(show);
        }
    })
});