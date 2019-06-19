DM.AttributeSpecificationControl = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.AttributeSpecificationControl.View(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("disabled", function() {
            this.disabled = true;
            this.trigger("disabled");
        }, this);
    },
    massageFormData: function () {
        this._view.showConstraints();
        this._view.massageFormData();
    },
    toggleRemoveLink: function(show) {
        this._view.toggleRemoveLink(show);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._index = options.index;
            this._block = options.block;
            this._animationTime = options.animationTime || 200;

            this._typeDropdown = this._block.find(".attribute-type-dropdown").dropdown(true);
            this._constraints = this._block.find(".attribute-constraints-wrapper");
            this._constraintsTypes = this._block.find(".attribute-constraints-type");
            this._constraintsToggleLink = this._block.find(".attribute-constraints-title");

            this._activeInput = this._block.find(".js-specification-active-input");
            this._removeLink = this._block.find(".js-remove-attribute-specification");
            this._reorderLink = this._block.find(".js-reorder-attribute-specification");

            this._listConstraintsContainer = this._block.find(".js-attribute-constraints-value-list");
            this._scaleConstraintsContainer = this._block.find(".js-attribute-constraints-value-scale");
            this._listConstraintsAddLink = this._block.find(".js-attribute-constraints-value-list-add");
            this._scaleConstraintsAddLink = this._block.find(".js-attribute-constraints-value-scale-add");
            
            this._cloneMachine = new DM.CloneMachine({
                reinitializeFormValidation: true,
                nodePatterns: {
                    "list": {
                        node: this._listConstraintsContainer.find(".attribute-constraints-list-item").first()
                    },
                    "scale": {
                        node: this._scaleConstraintsContainer.find(".attribute-constraints-list-item").first()
                    }
                }
            });
            this._cloneMachine.setCloneNodesCount("list", this._listConstraintsContainer.find(".attribute-constraints-list-item").length - 1);
            this._cloneMachine.setCloneNodesCount("scale", this._scaleConstraintsContainer.find(".attribute-constraints-list-item").length - 1);

            this._initSortables();

            this._resolveRemoveLinks(this._listConstraintsContainer);
            this._resolveRemoveLinks(this._scaleConstraintsContainer);
            
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._typeDropdown.on("select", function(option) {
                this._displayTypeConstraints(option.data("value"));
            }, this);
            this._constraintsToggleLink.on("click", function () {
                _this._toggleConstraints();
            });

            this._listConstraintsAddLink.on("click", function() {
                _this._cloneMachine.clone("list", function() {
                    this
                        .appendTo(_this._listConstraintsContainer)
                        .hide()
                        .slideDown(_this._animationTime);
                    this.find(".js-specification-valuesList-active-input").val("True");
                    _this._resolveRemoveLinks(_this._listConstraintsContainer);
                });
            });
            this._scaleConstraintsAddLink.on("click", function () {
                _this._cloneMachine.clone("scale", function() {
                    this
                        .appendTo(_this._scaleConstraintsContainer)
                        .hide()
                        .slideDown(_this._animationTime);
                    this.find("input[type='text']").val("");
                    this.find(".js-specification-valuesList-active-input").val("True");
                    _this._resolveRemoveLinks(_this._scaleConstraintsContainer);
                });
            });

            this._listConstraintsContainer.on("click", ".js-remove-attribute-value", function () {
                _this._disableListValue($(this).closest(".attribute-constraints-list-item"));
            });
            this._scaleConstraintsContainer.on("click", ".js-remove-attribute-value", function () {
                _this._disableListValue($(this).closest(".attribute-constraints-list-item"));
            });

            this._removeLink.on("click", function() {
                _this._disable();
            });
        },
        showConstraints: function () {
            if (this._constraints.css("display") === "none") {
                this._toggleConstraints();
            }
        },
        massageFormData: function() {
            this._listConstraintsContainer
                .find(".js-specification-valuesList-order-input")
                .each(function(i) { $(this).val(i); });
            this._scaleConstraintsContainer
                .find(".js-specification-valuesList-order-input")
                .each(function(i) { $(this).val(i); });
        },
        toggleRemoveLink: function (show) {
            this._removeLink.toggle(show);
            this._reorderLink.toggle(show);
        },
        _toggleConstraints: function () {
            this._constraintsToggleLink.find(".attribute-constraints-title-arrow").toggle();
            this._constraints.slideToggle(200);
        },
        _displayTypeConstraints: function (type) {
            var toDisplay = this._constraintsTypes.filter("[data-constraints-type='" + type + "']");
            this._constraintsTypes.not(toDisplay).slideUp(200);
            toDisplay.slideDown(200);
            this.showConstraints();
        },
        _disable: function () {
            this.trigger("disabled");
            this._activeInput.val("False");
            this._block.slideUp(200);
        },
        _disableListValue: function (listItem) {
            listItem.find(".js-specification-valuesList-active-input").val("False");
            listItem.slideUp(200);
            listItem.addClass("attribute-constraints-list-item-disabled");

            this._resolveRemoveLinks(listItem.closest(".js-attribute-constraints-values"));
        },
        _resolveRemoveLinks: function(container) {
            var showLinks = container.find(".attribute-constraints-list-item").not(".attribute-constraints-list-item-disabled").length > 1;
            container.find(".js-remove-attribute-value").toggle(showLinks);
            container.find(".js-reorder-attribute-value").toggle(showLinks);
        },
        _initSortables: function() {
            var options = {
                revert: 200,
                handle: ".js-reorder-attribute-value",
                axis: "y",
                tolerance: "pointer",
                placeholder: "attribute-specification-placeholder",
                forcePlaceholderSize: true
            };
            this._listConstraintsContainer.sortable(options);
            this._scaleConstraintsContainer.sortable(options);
        }
    })
});