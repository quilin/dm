DM.NotificationControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.NotificationControl.View(options);
        this._proxy = proxy || new DM.NotificationControl.Proxy(options);
    }
});