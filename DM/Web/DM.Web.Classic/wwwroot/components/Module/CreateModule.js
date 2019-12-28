DM.CreateModuleControl = Base.extend({
    constructor: function(options, view, proxy, createSchemaControlFactory) {
        this._view = view || new DM.CreateModuleControl.View(options);
        this._proxy = proxy || new DM.CreateModuleControl.Proxy(options);
        this._createSchemaControlFactory = createSchemaControlFactory || new DM.CreateModuleControl.CreateSchemaControlFactory(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("schemaCreate", function() {
            if (this._createSchemaControl !== undefined) {
                this._createSchemaControl.show();
            } else {
                this._proxy.getCreateSchemaForm();
            }
        }, this);
        this._proxy.on("getCreateSchemaFormRequestBegin", this._view.getCreateSchemaFormRequestBegin, this._view);
        this._proxy.on("getCreateSchemaFormRequestComplete", this._view.getCreateSchemaFormRequestComplete, this._view);
        this._proxy.on("getCreateSchemaFormRequestSuccess", function(data) {
            this._createSchemaControl = this._createSchemaControlFactory.create(data);
            this._createSchemaControl.on("schemaCreated", this._view.addSchema, this._view);
        }, this);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            var attributeSchemaSelect = $("#CreateModuleForm_AttributeSchemaId");
            this._schemaSelect = attributeSchemaSelect.dropdown(true);
            this._schemaLoader = DM.Loader.create(attributeSchemaSelect);

            this._extendedOptionsLink = $("#ExtendedOptionsLink");
            this._extendedOptionsBlock = $("#ExtendedOptions");

            this._assistantAutocomplete = DM.Autocomplete.create($("#AssistantAutocomplete"));

            this._form = $("#CreateModuleForm");
            this._attributeSchemaValidationMessage = $("#AttributeSchemaValidationMessage");

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

            this._schemaSelect.on("select", function (option, evt) {
                if (option.data("value") === DM.GuidFactory.empty) {
                    evt.preventDefault();
                    _this.trigger("schemaCreate");
                }
                _this._hideAttributeSchemaValidationMessage();
            });
            
            this._form.on("submit", function(evt) {
                if ($("#CreateModuleForm_AttributeSchemaId_Hidden").val() === DM.GuidFactory.empty) {
                    evt.preventDefault();
                    _this._showAttributeSchemaValidationMessage();
                }
            });
        },
        getCreateSchemaFormRequestBegin: function() {
            this._schemaLoader.show();
        },
        getCreateSchemaFormRequestComplete: function() {
            this._schemaLoader.hide();
        },
        addSchema: function(data) {
            this._schemaSelect.addOption(data);
            this._schemaSelect.selectOption(data.order);
        },
        _hideAttributeSchemaValidationMessage: function() {
            this._attributeSchemaValidationMessage.css({"display": "none"});
        },
        _showAttributeSchemaValidationMessage: function() {
            this._attributeSchemaValidationMessage.css({ "display": "inline-block" });
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._url = options.getCreateSchemaFormUrl;
        },
        getCreateSchemaForm: function () {
            $.ajax({
                type: "GET",
                url: this._url,
                context: this,
                beforeSend: this.handle("getCreateSchemaFormRequestBegin"),
                complete: this.handle("getCreateSchemaFormRequestComplete"),
                success: this.handle("getCreateSchemaFormRequestSuccess")
            });
        }
    }),
    CreateSchemaControlFactory: Base.extend({
        create: function(data, openLink) {
            return new DM.CreateAttributeSchemaControl({
                lightbox: DM.Lightbox.create(data, {
                    openLink: openLink
                })
            });
        }
    })
});