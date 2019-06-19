DM.PromptControl = Base.extend({
    constructor: function (view) {
        this._view = view || new DM.PromptControl.View();
        this._view
            .off("rejected")
            .off("resolved")
            .on("rejected", this.handle("rejected"), this)
            .on("resolved", this.handle("resolved"), this);
    },
    init: function (text, defaultValue) {
        this._view.init(text, defaultValue);
    }
}, {
    View: Base.extend({
        constructor: function () {
            this._lightbox = DM.Lightbox.create($("#__Prompt__"));
            this._value = $("#__Prompt__Value");
            this._submit = $("#__Prompt__True");
            this._text = $("#__Prompt__Text");

            var _this = this;

            this._lightbox.on("closed", function () {
                _this.trigger("rejected");
                _this._clear();
            }, this);
            this._submit.on("click", function () {
                _this._resolve(_this._value.val());
            });
            this._value.on("keydown", function(evt) {
                var btn = evt.keyCode || evt.which;
                if (btn === 13) {
                    evt.preventDefault();
                    _this._resolve(this.value);
                }
            });
        },
        init: function (text, defaultValue) {
            this._text.text(text);
            this._value.val(defaultValue);
            this._value[0].selectionStart = 0;
            this._value[0].selectionEnd = defaultValue.length;
            this._lightbox.open();
            this._value.focus();
        },
        _resolve: function (value) {
            this.trigger("resolved", value);
            this._lightbox.close();
            this._clear();
        },
        _clear: function () {
            this._text.text("");
            this._value.val("");
        }
    })
});

DM.Prompt = function (text, defaultValue) {
    var dfd = $.Deferred();

    this._prompt = this._prompt || new DM.PromptControl();

    this._prompt
        .off("rejected")
        .off("resolved")
        .on("rejected", dfd.reject, dfd)
        .on("resolved", dfd.resolve, dfd)
        .init(text, defaultValue);

    return dfd.promise();
}