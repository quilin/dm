DM.ConfirmControl = Base.extend({
    constructor: function(view) {
        this._view = view || new DM.ConfirmControl.View();
        this._view
            .off("rejected")
            .off("resolved")
            .on("rejected", this.handle("rejected"), this)
            .on("resolved", this.handle("resolved"), this);
    },
    init: function (message) {
        this._view.init(message);
    }
}, {
    View: Base.extend({
        constructor: function() {
            this._lightbox = DM.Lightbox.create($("#__Confirm__"));
            this._confirm = $("#__Confirm__True");
            this._cancel = $("#__Confirm__False");
            this._confirmText = $("#__Confirm__Text");
            this._confirmTitle = $("#__Confirm__Title");

            var _this = this;

            this._lightbox.on("closed", this.handle("rejected"), this);
            this._confirm.on("click", function () {
                _this.trigger("resolved");
                _this._lightbox.close();
            });
        },
        init: function(message, options) {
            options = options || {};

            this._lightbox.open();
            var messageText = message || "То, что вы пытаетесь сделать, может привести к необратимым последствиям. Вы уверены, что хотите продолжить?";
            var titleText = options.title || "Вы уверены?";
            var confirmButtonText = options.confirmText || "Да, продолжить";
            var cancelButtontext = options.cancelText || "Отменить";

            this._confirmText.html(messageText);
            this._confirmTitle.html(titleText);
            this._confirm.val(confirmButtonText);
            this._cancel.text(cancelButtontext);
        }
    })
});

DM.Confirm = function (message, options) {
    var dfd = $.Deferred();

    this._confirm = this._confirm || new DM.ConfirmControl();

    this._confirm
        .off("rejected")
        .off("resolved")
        .on("rejected", dfd.reject, dfd)
        .on("resolved", dfd.resolve, dfd)
        .init(message, options);

    return dfd.promise();
};