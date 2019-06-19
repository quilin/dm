DM.PostsListControl = DM.PagedListControl.extend({
    constructor: function (options, view, entityFactory, createEntityControl, proxy) {
        this._proxy = proxy || new DM.PostsListControl.Proxy(options);
        this.base(options,
            view || new DM.PostsListControl.View(options),
            entityFactory || new DM.PostsListControl.PostEntityFactory(options),
            createEntityControl || new DM.CreatePostsListEntityControl(options));
    },
    __attachEventListeners: function () {
        this.base();
        this._createEntityControl.on("created", this._proxy.updateNotifications, this._proxy);

        // Updates of notified users list - triggers
        this.on("editedEntity", this._proxy.updateNotifications, this._proxy);
        this._view.on("removeNotificationRequest", this._proxy.removeNotifications, this._proxy);

        // Updates of notified users list - loading data
        this._proxy.on("updateNotificationsBegin", this._updateNotificationsBegin, this);
        this._proxy.on("updateNotificationsComplete", this._updateNotificationsComplete, this);
        this._proxy.on("updateNotificationsSuccess", this._updateNotificationsSuccess, this);
    },
    _updateNotificationsBegin: function() {
        this._view.updateNotificationsBegin();
    },
    _updateNotificationsComplete: function() {
        this._view.updateNotificationsComplete();
    },
    _updateNotificationsSuccess: function (data) {
        this._view.updateNotificationsSuccess(data);
    }
}, {
    View: DM.PagedListControl.View.extend({
        constructor: function (options) {
            this._notificationsList = $("#NotificationsList");
            this._notificationsLoader = DM.Loader.create(this._notificationsList);
            this._roomNotification = $("#RoomNotification");

            this.base(options);
        },
        updateNotificationsBegin: function() {
            this._notificationsLoader.show();
        },
        updateNotificationsComplete: function() {
            this._notificationsLoader.hide();
        },
        updateNotificationsSuccess: function (data) {
            this._notificationsList.html(data);
            if (!data) {
                this._roomNotification.remove();
            }
        },
        __attachEventListeners: function () {
            this.base();
            var _this = this;
            this._notificationsList.on("click", ".roomNotification-deleteLink", function (evt) {
                evt.preventDefault();
                _this.trigger("removeNotificationRequest", this.href);
            });
        }
    }),
    Proxy: Base.extend({
        constructor: function(options) {
            this._updateNotificationsUrl = options.updateNotificationsUrl;
        },
        updateNotifications: function () {
            $.ajax({
                type: "POST",
                url: this._updateNotificationsUrl,
                context: this,
                beforeSend: this.handle("updateNotificationsBegin"),
                complete: this.handle("updateNotificationsComplete"),
                success: this.handle("updateNotificationsSuccess")
            });
        },
        removeNotifications: function (url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("updateNotificationsBegin"),
                complete: this.handle("updateNotificationsComplete"),
                success: this.handle("updateNotificationsSuccess")
            });
        }
    }),
    PostEntityFactory: DM.PagedListControl.EntityFactory.extend({
        create: function (entityId) {
            return new DM.PostEntityControl({
                entityId: entityId
            });
        }
    })
});