DM.PostEntityControl = DM.PagedListEntityControl.extend({
    constructor: function(options, view, proxy, editEntityControlFactory, voteFormFactory, warningsList) {
        this._voteFormFactory = voteFormFactory || new DM.PostEntityControl.VoteFormFactory(options);
        this.base(
            options,
            view || new DM.PostEntityControl.View(options),
            proxy || new DM.PostEntityControl.Proxy(options),
            editEntityControlFactory || new DM.PostEntityControl.EditEntityControlFactory(options),
            warningsList || new DM.EntityWarningsListControl(options));
    },
    __attachEventListeners: function() {
        this.base();

        this._view.on("voteFormRequest", this._getVoteForm, this);
        this._proxy.on("voteFormRequestBegin", this._view.voteFormRequestBegin, this._view);
        this._proxy.on("voteFormRequestComplete", this._view.voteFormRequestComplete, this._view);
        this._proxy.on("voteFormRequestError", this._view.voteFormRequestError, this._view);
        this._proxy.on("voteFormRequestSuccess", this._voteFormRequestSuccess, this);
    },
    _getVoteForm: function (url) {
        if (this._voteForm) {
            this._voteForm.dispose();
        }

        this._proxy.getVoteForm(url);
    },
    _voteFormRequestSuccess: function(data) {
        this._voteForm = this._voteFormFactory.create(data);
        this._voteForm.open();

        this._voteForm.on("voteSuccess", function() {
            this._view.voteSuccess();
            this._voteForm.dispose();
        }, this);
    }
}, {
    View: DM.PagedListEntityControl.View.extend({
        __attachEventListeners: function () {
            this.base();

            this._voteActions = $("#VoteActions_" + this._entityId);
            this._voteActionsLoader = DM.Loader.create(this._voteActions);

            var _this = this;
            this._voteActions.on("click.request", ".js-vote-link", function (evt) {
                evt.preventDefault();
                _this.trigger("voteFormRequest", this.href);
            });
        },
        voteFormRequestBegin: function () {
            this._voteActionsLoader.show();
        },
        voteFormRequestComplete: function() {
            this._voteActionsLoader.hide();
        },
        voteFormRequestError: function (data) {
            var errorData = JSON.parse(data.responseText);
            DM.Alert(errorData.message);
        },
        voteSuccess: function() {
            this._voteActions.remove();
            this._voteActionsLoader.remove();
        }
    }),
    VoteFormFactory: Base.extend({
        constructor: function(options) {
            this._postId = options.entityId;
        },
        create: function(data) {
            return new DM.VoteFormControl({
                postId: this._postId,
                data: data
            });
        }
    }),
    EditEntityControlFactory: DM.PagedListEntityControl.EditEntityControlFactory.extend({
        create: function () {
            return new DM.EditPostEntityControl({
                entityId: this._entityId
            });
        }
    }),
    Proxy: DM.PagedListEntityControl.Proxy.extend({
        getVoteForm: function(url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("voteFormRequestBegin"),
                complete: this.handle("voteFormRequestComplete"),
                error: this.handle("voteFormRequestError"),
                success: this.handle("voteFormRequestSuccess")
            });
        }
    })
});