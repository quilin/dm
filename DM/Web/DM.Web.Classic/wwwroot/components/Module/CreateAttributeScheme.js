DM.CreateAttributeSchemeControl = Base.extend({
    constructor: function(options, view, specificationControlFactory) {
        this._view = view || new DM.CreateAttributeSchemeControl.View(options);
        this._specificationControlFactory = specificationControlFactory || new DM.CreateAttributeSchemeControl.SpecificationControlFactory(options);

        this._specifications = [];
        this._prefix = this._view.getPrefix();
        this._animationTime = options.animationTime;

        this.__init();
        this.__attachEventListeners();
    },
    __init: function () {
        var specificationBlocks = this._view.getSpecificationBlocks();
        for (var i = 0; i < specificationBlocks.length; ++i) {
            this.addSpecification(specificationBlocks[i]);
        }
    },
    __attachEventListeners: function() {
        this._view.on("schemeCreated", this.schemeCreated, this);
        this._view.on("schemeValidation", this.massageFormData, this);
        this._view.on("specificationAdded", this.addSpecification, this);
    },
    schemeCreated: function (data) {
        if (data) {
            this._view.hide();
            this.trigger("schemeCreated", data);
        } else {
            this._view.reloadPage();
        }
    },
    show: function () {
        this._view.show();
    },
    massageFormData: function () {
        this._view.massageFormData();
        for (var i = 0; i < this._specifications.length; ++i) {
            this._specifications[i].massageFormData();
        }
    },
    addSpecification: function(block) {
        var prefix = this._view.getPrefix();
        var index = this._specifications.length;
        var specification = this._specificationControlFactory.create(block, index, prefix, this._animationTime);
        this._specifications.push(specification);

        specification.on("disabled", this._resolveRemoveLinks, this);
        this._resolveRemoveLinks();
    },
    _resolveRemoveLinks: function () {
        var showLinks = this._specifications.count(function (s) { return !s.disabled; }) > 1;
        this._specifications.each(function(s) { s.toggleRemoveLink(showLinks) });
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._lightbox = options.lightbox;
            this._lightbox && this._lightbox.open();

            this._animationTime = options.animationTime || 200;

            this._form = new DM.FormControl("#CreateAttributeSchemeForm", {
                validate: true,
                initPlaceholder: true
            });

            this._confirmButton = $("#CreateAttributeSchemeConfirm");
            this._saveAsButton = $("#SaveAttributeSchemeAs");
            
            this._specificationsContainer = $("#SpecificationsContainer");
            this._addSpecificationLink = $("#AddSpecification");
            this._specificationBlock = $(".js-specification-block").first();
            this._specificationCloneMachine = new DM.CloneMachine({
                reinitializeFormValidation: true,
                cascadeClone: true,
                nodePatterns: {
                    "specification": {
                        node: this._specificationBlock
                    }
                }
            });
            this._specificationCloneMachine.setCloneNodesCount("specification", $(".js-specification-block").length - 1);

            this._initSortable();

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;

            this._addSpecificationLink.on("click.clone", function() {
                _this._specificationCloneMachine.clone("specification", function() {
                    this
                        .appendTo(_this._specificationsContainer)
                        .hide()
                        .slideDown(_this._animationTime);
                    this.find(".js-specification-id-input").val("");
                    this.find(".js-specification-active-input").val("True");
                }, function() {
                    _this.trigger("specificationAdded", this);
                });
            });

            this._form
                .on("requestValidation", this.handle("schemeValidation"), this)
                .on("requestSuccess", this.handle("schemeCreated"), this);

            this._confirmButton.on("click", function() {
                var confirmMessage = "Изменения в этой схеме могут затронуть другие игры, где вы ее используете. Возможно, вы хотите создать другую схему на основе этой.<br><br>Вы уверены, что хотите сохранить изменения?";
                DM.Confirm(confirmMessage).done(function () {
                    _this._form.submit();
                });
            });
            this._saveAsButton.on("click", function () {
                var $this = $(this);
                var action = $this.data("action");
                var schemeTitle = $("#SchemeTitle");
                if (schemeTitle.val() === schemeTitle[0].defaultValue) {
                    DM.Prompt($this.data("promptText"), $("#SchemeTitle").val() + " (1)").done(function (newName) {
                        _this._form.$().attr("action", action);
                        _this._form.$().find("#SchemeTitle").val(newName);
                        _this._form.submit();
                        _this._savedAs = true;
                    });
                } else {
                    _this._form.$().attr("action", action);
                    _this._form.submit();
                    _this._savedAs = true;
                }
            });
        },
        show: function () {
            this._lightbox && this._lightbox.open();
        },
        hide: function() {
            this._lightbox && this._lightbox.close();
        },
        getSpecificationBlocks: function() {
            return $(".js-specification-block").mapAsArray();
        },
        getPrefix: function() {
            return this._specificationBlock.data("prefix");
        },
        massageFormData: function() {
            this._form.$()
                .find(".js-specification-order-input")
                .each(function (i) { $(this).val(i); });
            this._form.resolveEnabled();
        },
        reloadPage: function() {
            document.location.href = document.location.href;
        },
        _initSortable: function() {
            var _this = this;
            this._specificationsContainer.sortable({
                revert: 200,
                handle: ".js-attribute-draggable",
                axis: "y",
                tolerance: "pointer",
                placeholder: "attribute-specification-placeholder",
                forcePlaceholderSize: true,
                stop: function() {
                    _this.massageFormData();
                }
            });
        },
    }),
    SpecificationControlFactory: Base.extend({
        create: function(block, index, prefix, animationTime) {
            return new DM.AttributeSpecificationControl({
                index: index,
                block: block,
                prefix: prefix,
                animationTime: animationTime
            });
        }
    })
});