DM.TopicActionsControl = Base.extend({
    constructor: function (options, view, proxy) {
        this._view = view || new DM.TopicActionsControl.View(options);
        this._proxy = proxy || new DM.TopicActionsControl.Proxy(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view
            .on("action", this._proxy.performAction, this._proxy)
            .on("moveRequest", this._proxy.move, this._proxy);
        this._proxy.on("actionSuccess", this._view.reloadPage, this._view);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._topicId = options.topicId;
            
            this._actionsLink = $("#ShowTopicActionsLink");
            this._actionsBlock = $("#TopicActions");
            this._actionsPopup = new DM.Popup({
                bindItem: this._actionsLink,
                popupItem: this._actionsBlock,
                position: {
                    right: 0,
                    top: this._actionsLink.height()
                }
            });
            
            this._moveDropdown = $("#MoveTopicDropdown").dropdown(true);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._actionsBlock.on("click.request", ".js-topic-action", function () {
                _this.trigger("action", this.getAttribute("data-url"));
            });
            this._actionsLink.on("click.showActions", function () {
                _this._actionsPopup.show();
            });
            this._moveDropdown.on("select", function (option) {
                this.trigger("moveRequest", {
                    forumId: option.data("value"),
                    topicId: this._topicId
                });
            }, this);
        },
        reloadPage: function () {
            document.location.href = document.location.href;
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._moveUrl = options.moveUrl;
        },
        performAction: function (url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                success: this.handle("actionSuccess")
            })
        },
        move: function (data) {
            $.ajax({
                type: "POST",
                url: this._moveUrl,
                data: data, 
                context: this,
                success: this.handle("actionSuccess")
            });
        }
    })
});