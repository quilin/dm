DM.UploadPopup = DM.PopupBase.extend({
    constructor: function(options, view) {
        options.fixed = true;
        options.position = { top: 0, left: 0 };
        this.base(options, view || new DM.UploadPopup.View(options));
    },
    show: function() {
        this._setSize(this._view.getBindItemSize());
        this.base();
    }
}, {
    View: DM.PopupBase.View.extend({
        getBindItemSize: function () {
            return {
                width: this._bindItem.outerWidth(),
                height: this._bindItem.outerHeight()
            };
        }
    })
});