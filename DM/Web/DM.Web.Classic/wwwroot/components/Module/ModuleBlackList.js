DM.ModuleBlackListControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.ModuleBlackListControl.View(options);
        this._proxy = proxy || new DM.ModuleBlackListControl.Proxy(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("removeRequest", this._proxy.removeEntry, this._proxy);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._createForm = new DM.FormControl("#CreateBlackListForm");
            this._autocomplete = DM.Autocomplete.create($("#BlackListAutocomplete"));
            this._input = $("#CreateBlackListInput");
            
            this._container = $("#BlackListContainer");
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            this._autocomplete.on("select", function (option) {
                this._input.val(option.data("value"));
                this._createForm.submit();
                this._createForm.reset();
            }, this);
            this._createForm.on("requestSuccess", function (data) {
                this._container.append(data);
            }, this);
            
            var _this = this;
            this._container.on("click", ".dropdownmultiselect-selectedList-deselectLink", function (evt) {
                evt.preventDefault();
                _this.trigger("removeRequest", this.href);
                $(this).parent().remove();
            });
        }
    }),
    Proxy: Base.extend({
        removeEntry: function (url) {
            $.ajax({
                type: "POST",
                url: url
            });
        }
    })
});