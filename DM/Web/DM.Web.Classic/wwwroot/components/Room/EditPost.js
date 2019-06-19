DM.EditPostEntityControl = DM.EditPagedListEntityControl.extend({
    constructor: function(options, view) {
        this._view = view || new DM.EditPostEntityControl.View(options);
        this.base(options, this._view);
    }
}, {
    View: DM.EditPagedListEntityControl.View.extend({
        constructor: function (options) {
            var $dropdownBind = $("#EditPostNotifyUsers_" + options.entityId);
            if ($dropdownBind.length > 0) {
                this._notifyUserIds = new DM.DropdownMultiSelect({
                    bindItem: $dropdownBind,
                    defaultLabel: "Жду сообщения от..."
                });
            }
            this._diceControl = new DM.DiceControl({
                postId: options.entityId
            });

            this.base(options);

            var _this = this;
            this._form.interfere(function (data) {
                return data + "&" + _this._diceControl.getDiceIdsEncoded();
            });
        },
        __attachEventListeners: function () {
            if (this._notifyUserIds) {
                this._notifyUserIds.on("valueChanged", this._form.resolveEnabled, this._form);
            }
            this._diceControl.on("diceChanged", this._form.resolveEnabled, this._form);


            this.base();
        }
    })
});