DM.VoteFormControl = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.VoteFormControl.View(options);
        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("voteSuccess", this.handle("voteSuccess"), this);
    },
    open: function() {
        this._view.open();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._lightbox = DM.Lightbox.create(options.data);
            this._postId = options.postId;
            this._form = new DM.FormControl("#VoteForm_" + this._postId, {
                validate: true
            });

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            this._form.on("requestSuccess", function() {
                this._lightbox.close();
                this.trigger("voteSuccess");
            }, this);
        },
        open: function() {
            this._lightbox.open();
        }
    })
});