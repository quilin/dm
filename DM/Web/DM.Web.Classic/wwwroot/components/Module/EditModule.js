DM.EditModuleControl = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.EditModuleControl.View(options);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._block = $("#GeneralSettingsTab");

            this._tagIds = new DM.DropdownMultiSelect({
                bindItem: $("#EditModuleTags"),
                defaultLabel: "Выберите теги"
            });

            this._assistantAutocomplete = DM.Autocomplete.create($("#AssistantAutocomplete"));
        }
    })
});