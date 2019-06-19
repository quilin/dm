DM.DiceControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.DiceControl.View(options);
        this._proxy = proxy || new DM.DiceControl.Proxy(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("request", this._proxy.rollDice, this._proxy);
        this._proxy.on("requestBegin", this._view.requestBegin, this._view);
        this._proxy.on("requestComplete", this._view.requestComplete, this._view);
        this._proxy.on("requestSuccess", this._view.addDiceResult, this._view);

        this._view.on("diceChanged", this.handle("diceChanged"), this);
    },
    getDiceIdsEncoded: function() {
        return this._view.getDiceIdsEncoded();
    },
    clear: function() {
        this._view.clear();
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            var elementPostfix = options.postId ? "_" + options.postId : "";
            this._diceThrowerBlock = $("#DiceContainer" + elementPostfix);
            this._diceResultContainer = $("#DiceResultContainer" + elementPostfix);
            this._rollButton = this._diceThrowerBlock.find(".js-rollButton");

            this._rollsCountInput = this._diceThrowerBlock.find(".js-dice-throws-input");
            this._edgesCountInput = this._diceThrowerBlock.find(".js-dice-edges-input").suggest();
            this._bonusInput = this._diceThrowerBlock.find(".js-dice-bonus-input");
            if (this._bonusInput.hasClass("dds-suggest-input")) {
                this._bonusInput.suggest();
            }

            this._blastCountInput = this._diceThrowerBlock.find(".js-dice-blast-input");
            this._commentaryInput = this._diceThrowerBlock.find(".js-dice-comment-input");
            this._fairCheckbox = this._diceThrowerBlock.find(".js-dice-fair");
            this._hiddenCheckbox = this._diceThrowerBlock.find(".js-dice-hidden");

            this._loader = DM.Loader.create(this._diceThrowerBlock);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._diceResultContainer.on("click.remove", ".dice-removeLink", function () {
                $(this).closest(".dice-result-item").remove();
                _this.trigger("diceChanged");
            });
            this._rollButton.on("click.roll", function() {
                if (_this._diceThrowerBlock.find(".input-validation-error").length === 0) {
                    var data = {
                        isHidden: _this._hiddenCheckbox[0].checked,
                        rollsCount: _this._rollsCountInput.val(),
                        edgesCount: _this._edgesCountInput.val(),
                        blastCount: _this._blastCountInput.val(),
                        bonus: _this._bonusInput.val(),
                        commentary: _this._commentaryInput.val()
                    };
                    if (_this._fairCheckbox.length > 0) {
                        data.isFair = _this._fairCheckbox[0].checked;
                    }

                    _this.trigger("request", $(this).data("url"), data);
                }
            });
            this._diceThrowerBlock.on("keydown.uglyHack", ".js-non-submit-input", function(evt) {
                if (evt.keyCode === 27) {
                    evt.preventDefault();
                }
            });
        },
        requestBegin: function () {
            this._loader.show();
        },
        requestComplete: function() {
            this._loader.hide();
        },
        addDiceResult: function (data) {
            this._diceResultContainer.append(data);
            this._commentaryInput.val("");
            this.trigger("diceChanged");
        },
        getDiceIdsEncoded: function () {
            var prefix = this._diceResultContainer.data("prefix");
            var ids = [];
            this._diceResultContainer.find(".dice-result-item").each(function() {
                ids.push($(this).data("diceId"));
            });
            return ids.map(function(id, i) {
                return prefix + "[" + i + "]=" + id;
            }).join("&");
        },
        clear: function() {
            this._diceResultContainer.empty();
        }
    }),
    Proxy: Base.extend({
        rollDice: function(url, data) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                beforeSend: this.handle("requestBegin"),
                complete: this.handle("requestComplete"),
                success: this.handle("requestSuccess")
            });
        }
    })
});