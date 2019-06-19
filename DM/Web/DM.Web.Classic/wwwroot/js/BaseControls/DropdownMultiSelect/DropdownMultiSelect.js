DM.DropdownMultiSelect = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.DropdownMultiSelect.View(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("input.formReset", this.handle("input.formReset"), this);
        this._view.on("valueChanged", this.handle("valueChanged"), this);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._bindItem = options.bindItem;
            this._defaultLabel = options.defaultLabel;

            this._dropdown = this._bindItem.find(".dropdownmultiselect-dropdown").dropdown(true, {
                remainOpenOnSelect: true
            });

            this._selectedList = this._bindItem.find(".dropdownmultiselect-selectedList");
            this._nameFormat = this._bindItem.data("name-format");
            this._deselectFormat = this._bindItem.find(".dropdownmultiselect-deselectFormat").html();
            this._preselected = this._bindItem.data("preselected");

            this.__init();

            this.__attachEventListeners();
        },
        __init: function () {
            this._selectedList.empty();
            this._setDropdownValue();
            this._preselect();
        },
        __attachEventListeners: function() {
            this._dropdown.on("input.formReset", function() {
                this.__init();
                this.trigger("input.formReset");
            }, this);

            this._dropdown.on("select", function(option) {
                this._setDropdownValue();
                this._addSelectedEntry(option.data("value"), option.text());
            }, this);

            var _this = this;
            this._selectedList.on("click", ".dropdownmultiselect-selectedList-deselectLink", function (event) {
                event.preventDefault();
                $(this).parent().remove();
                _this.trigger("valueChanged");
            });
        },
        _setDropdownValue: function() {
            this._dropdown.setValue(this._defaultLabel);
        },
        _addSelectedEntry: function(id, label) {
            if (this._selectedList.children("[data-id='" + id + "']").length < 1) {
                var nextId = this._selectedList.children().length;
                var nextName = this._nameFormat.replace("0", nextId);
                var $selectedItemTag = $("<div/>", {
                    "class": "dropdownmultiselect-selectedList-wrapper",
                    "data-id": id
                });
                var $input = $("<input/>", {
                    "type": "hidden",
                    "name": nextName,
                    "value": id
                });
                var $removeLink = $("<a/>", {
                    "class": "dropdownmultiselect-selectedList-deselectLink",
                    "href": "javascript:void(0)"
                });
                $removeLink.html(this._deselectFormat);
                $selectedItemTag.append(label, $removeLink, $input);
                this._selectedList.append($selectedItemTag);
                this.trigger("valueChanged");
            }
        },
        _preselect: function () {
            var options = this._dropdown.getOptions();
            for (var preselectedId = 0; preselectedId < this._preselected.length; preselectedId++) {
                for (var optionId = 0; optionId < options.length; optionId++) {
                    if (options[optionId].value === this._preselected[preselectedId]) {
                        this._addSelectedEntry(options[optionId].value, options[optionId].text);
                    }
                }
            }
        }
    })
});