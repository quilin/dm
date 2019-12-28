DM.ModuleSettingsControl = Base.extend({
    constructor: function (options, view) {
        this._view = view || new DM.ModuleSettingsControl.View(options);
        this._options = options;

        this._controls = {};
        this._controls[options.defaultControl + "SettingsTab"] = new this._controlLinks[options.defaultControl + "SettingsTab"](options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("controlLoaded", function(tabId) {
            if (this._controls[tabId] === undefined) {
                this._controls[tabId] = new this._controlLinks[tabId](this._options);
            }
        }, this);
    },
    _controlLinks: {
        "GeneralSettingsTab": DM.EditModuleControl,
        "RoomsSettingsTab": DM.RoomsListControl,
        "AttributesSettingsTab": DM.CreateAttributeSchemaControl,
        "NotepadSettingsTab": DM.ModuleNotepadControl,
        "BlackListSettingsTab": DM.ModuleBlackListControl,
        "RemoveSettingsTab": function() {}
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._tabs = new DM.TabsControl({
                tabLinkSelector: ".tabLink",
                tabBlocksSelector: ".tabBlock",
                defaultTabId: options.defaultTabId
            });

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            this._tabs.on("tabOpened", this.handle("controlLoaded"), this);
        }
    })
});