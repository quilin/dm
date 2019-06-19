DM.PersonalDataFieldControl = Base.extend({
    constructor: function(type, view) {
        this._view = view || new DM.PersonalDataFieldControl.View(type);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("editing", this.handle("editing"), this);
    },
    cancel: function () {
        this._view.cancel();
    }
}, {
    View: Base.extend({
        constructor: function (type) {
            this._showFormLink = $("#ShowFormLink_" + type);
            this._cancelFormLink = $("#CancelFormLink_" + type);
            this._dataFieldContainer = $("#DataFieldContainer_" + type);
            this._dataFieldValue = $("#DataFieldValue_" + type);
            this._form = new DM.FormControl("#EditDataFieldForm_" + type, {
                updateDefaultDataAfterSuccess: true
            });

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._showFormLink.on("click.showForm", function () {
                _this.trigger("editing");
                _this._toggleForm(true);
            });
            this._cancelFormLink.on("click.showForm", function() {
                _this._toggleForm(false);
            });
            this._form.on("requestSuccess", function(data) {
                _this._dataFieldValue.text(data || "не указано");
                _this._toggleForm(false);
            });
        },
        _toggleForm: function (show) {
            this._form.$().toggle(show);
            this._dataFieldContainer.toggle(!show);
            if (show) {
                this._form.$().find("input[type='text']").focus();
            } else {
                this._form.$().find("input").blur();
            }
        },
        cancel: function () {
            this._toggleForm(false);
        }
    })
});