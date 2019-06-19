DM.ModuleInfoControl = Base.extend({
    constructor: function(options, view, characters) {
        this._view = view || new DM.ModuleInfoControl.View(options);
        this._characters = characters || new DM.RoomCharactersControl();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._charactersList = new DM.OrderableTableControl({
                table: $("#CharactersList")
            });
            this._npcsTable = $("#NpcsList");
            this._toggleNpcsTableLink = $("#ToggleNpcsLink");
            this._npcsList = new DM.OrderableTableControl({
                table: this._npcsTable
            });

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._toggleNpcsTableLink.on("click.toggleNpcs", function() {
                _this._npcsTable.slideToggle();
                _this._toggleNpcsTableLink.toggleClass("hidden");
            });
        }
    })
})