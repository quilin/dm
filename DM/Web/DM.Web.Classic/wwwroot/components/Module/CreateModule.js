DM.CreateModuleControl = Base.extend({
    constructor: function(options, view, proxy, createSchemeControlFactory) {
        this._view = view || new DM.CreateModuleControl.View(options);
        this._proxy = proxy || new DM.CreateModuleControl.Proxy(options);
        this._createSchemeControlFactory = createSchemeControlFactory || new DM.CreateModuleControl.CreateSchemeControlFactory(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("schemeCreate", function() {
            if (this._createSchemeControl !== undefined) {
                this._createSchemeControl.show();
            } else {
                this._proxy.getCreateSchemeForm();
            }
        }, this);
        this._proxy.on("getCreateSchemeFormRequestBegin", this._view.getCreateSchemeFormRequestBegin, this._view);
        this._proxy.on("getCreateSchemeFormRequestComplete", this._view.getCreateSchemeFormRequestComplete, this._view);
        this._proxy.on("getCreateSchemeFormRequestSuccess", function(data) {
            this._createSchemeControl = this._createSchemeControlFactory.create(data);
            this._createSchemeControl.on("schemeCreated", this._view.addScheme, this._view);
        }, this);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            var attributeSchemeSelect = $("#CreateModuleForm_AttributeSchemeId");
            this._schemeSelect = attributeSchemeSelect.dropdown(true);
            this._schemeLoader = DM.Loader.create(attributeSchemeSelect);

            this._extendedOptionsLink = $("#ExtendedOptionsLink");
            this._extendedOptionsBlock = $("#ExtendedOptions");

            this._assistantAutocomplete = DM.Autocomplete.create($("#AssistantAutocomplete"));

            this._form = $("#CreateModuleForm");
            this._attributeSchemeValidationMessage = $("#AttributeSchemeValidationMessage");

            this._tagIds = new DM.DropdownMultiSelect({
                bindItem: $("#CreateModuleTags"),
                defaultLabel: "Выберите теги"
            });

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._extendedOptionsLink.on("click.showExtendedOptions", function () {
                _this._extendedOptionsBlock.show();
                _this._extendedOptionsLink.hide();
            });

            this._schemeSelect.on("select", function (option, evt) {
                if (option.data("value") === DM.GuidFactory.empty) {
                    evt.preventDefault();
                    _this.trigger("schemeCreate");
                }
                _this._hideAttributeSchemeValidationMessage();
            });
            
            this._form.on("submit", function(evt) {
                if ($("#CreateModuleForm_AttributeSchemeId_Hidden").val() === DM.GuidFactory.empty) {
                    evt.preventDefault();
                    _this._showAttributeSchemeValidationMessage();
                }
            });
        },
        getCreateSchemeFormRequestBegin: function() {
            this._schemeLoader.show();
        },
        getCreateSchemeFormRequestComplete: function() {
            this._schemeLoader.hide();
        },
        addScheme: function(data) {
            this._schemeSelect.addOption(data);
            this._schemeSelect.selectOption(data.order);
        },
        _hideAttributeSchemeValidationMessage: function() {
            this._attributeSchemeValidationMessage.css({"display": "none"});
        },
        _showAttributeSchemeValidationMessage: function() {
            this._attributeSchemeValidationMessage.css({ "display": "inline-block" });
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._url = options.getCreateSchemeFormUrl;
        },
        getCreateSchemeForm: function () {
            $.ajax({
                type: "GET",
                url: this._url,
                context: this,
                beforeSend: this.handle("getCreateSchemeFormRequestBegin"),
                complete: this.handle("getCreateSchemeFormRequestComplete"),
                success: this.handle("getCreateSchemeFormRequestSuccess")
            });
        }
    }),
    CreateSchemeControlFactory: Base.extend({
        create: function(data, openLink) {
            return new DM.CreateAttributeSchemeControl({
                lightbox: DM.Lightbox.create(data, {
                    openLink: openLink
                })
            });
        }
    })
});