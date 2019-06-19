DM.MasterControl = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.MasterControl.View(options);

        this._masterData = options.data || {};
        this._totalSteps = this._view.totalSteps;

        this._currentStep = 0;
        this._forceGoToStep(0);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view
            .on("goToStep", this._resolveStep, this)
            .on("goNext", this._resolveNext, this)
            .on("goPrev", this._resolvePrev, this);
    },
    _resolveNext: function() {
        this._resolveStep(this._currentStep + 1);
    },
    _resolvePrev: function() {
        this._resolveStep(this._currentStep - 1);
    },
    _resolveStep: function(stepNumber) {
        if (stepNumber < 0 ||
            stepNumber > this._totalSteps ||
            stepNumber === this._currentStep) {
            return;
        }

        if (this._currentStep > stepNumber) {
            this._forceGoToStep(stepNumber);
        } else {
            this._goToStep(stepNumber);
        }
    },

    _goToStep: function(stepNumber) {
        // todo: needs to be refactored once masters become more complex, every step should be its own control with its own responsibility on validation
        for (var i = this._currentStep; i < stepNumber; ++i) {
            if (!this._view.stepIsValid(i)) {
                this._forceGoToStep(i);
                return;
            }
        }
        this._forceGoToStep(stepNumber);
    },
    _forceGoToStep: function(stepNumber) {
        this._currentStep = stepNumber;
        this._view.goToStep(stepNumber);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._container = options.container;

            this._stepLinks = this._container.find(".js-master-step");
            this._stepContents = this._container.find(".js-master-content");

            this._contentWrapper = this._container.find(".js-master-content-wrapper");
            this._slider = this._container.find(".js-master-content-slider");

            this._nextLink = this._container.find(".js-master-actions-next");
            this._prevLink = this._container.find(".js-master-actions-prev");

            this.totalSteps = this._stepLinks.length - 1;

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._stepLinks.on("click.goToStep", function(evt) {
                var stepNumber = $(this).data("step");
                _this.trigger("goToStep", stepNumber);
            });
            this._nextLink.on("click.goNext", function() {
                _this.trigger("goNext");
            });
            this._prevLink.on("click.goPrev", function() {
                _this.trigger("goPrev");
            });
        },

        stepIsValid: function(stepNumber) {
            return true;
        },
        goToStep: function(stepNumber) {
            this._highlightStepLink(stepNumber);
            this._resizeSlider(stepNumber);
            this._resolveActions(stepNumber);
        },

        _highlightStepLink: function(stepNumber) {
            this._stepLinks.removeClass("active");
            $(this._stepLinks[stepNumber]).addClass("active");
        },
        _resizeSlider: function(stepNumber) {
            var stepContent = $(this._stepContents[stepNumber]);
            var height = stepContent.outerHeight();
            var width = stepContent.outerWidth();
            this._contentWrapper
                .stop()
                .animate({ height: height }, 200);
            this._slider
                .stop()
                .animate({ left: -width * stepNumber }, 200);
        },
        _resolveActions: function(stepNumber) {
            var submitText = $(this._stepContents[stepNumber]).data("stepSubmit");
            this._submit.text(submitText);
            this._prevLink.toggleClass("disabled", stepNumber === 0);
            this._nextLink.toggleClass("disabled", stepNumber === this.totalSteps);
        }
    })
});