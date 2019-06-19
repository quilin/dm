DM.CharacterControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.CharacterControl.View(options);
        this._proxy = proxy || new DM.CharacterControl.Proxy();

        this._infoDisplayed = false;

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("previewToggle", function() {
            if (this._infoDisplayed)
                this.hideInfo(true);
            else
                this.showInfo(true);
        }, this);
            
        this._view.on("characterActionRequest", this._proxy.characterAction, this._proxy);
        this._proxy.on("characterActionRequestBegin", this._view.characterActionRequestBegin, this._view);
        this._proxy.on("characterActionRequestComplete", this._view.characterActionRequestComplete, this._view);
        this._proxy.on("characterActionRequestSuccess", function (data) {
            if (data === "")
                this.remove();
            else
                this._view.characterActionRequestSuccess(data);
        }, this);
    },
    hideInfo: function(withAnimation) {
        this._infoDisplayed = false;
        this._view.hideInfo(withAnimation);
    },
    showInfo: function (withAnimation) {
        this._infoDisplayed = true;
        this._view.showInfo(withAnimation);
    },
    getItem: function () {
        return this._view.getItem();
    },
    remove: function () {
        this._view.remove();
        this.trigger("remove");
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._characterId = options.characterId;
            this._wrapper = $("#CharacterWrapper_" + this._characterId);
            this._previewBlock = $("#CharacterPreview_" + this._characterId);
            this._infoBlock = $("#CharacterInfo_" + this._characterId);
            this._actionsBlock = $("#CharacterActions_" + this._characterId);

            this._actionsLoader = DM.Loader.create(this._actionsBlock);

            this._animationTime = 300;
                
            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;

            this._previewBlock.on("click", function() {
                _this.trigger("previewToggle");
            });
            this._previewBlock.on("click", ".characterAction-link", function (evt) {
                evt.preventDefault();
                evt.stopPropagation();
                _this.trigger("characterActionRequest", this.href);
            });
            this._previewBlock.on("click", ".removeCharacter-link", function(evt) {
                evt.preventDefault();
                evt.stopPropagation();
                var href = this.href;
                DM.Confirm().done(function() {
                    _this.trigger("characterActionRequest", href);
                });
            });
            this._previewBlock.on("click", "a", function(evt) {
                evt.stopPropagation(); // edit link
            });
        },
        hideInfo: function (withAnimation) {
            if (withAnimation)
                this._infoBlock.stop().slideUp(this._animationTime);
            else
                this._infoBlock.hide();
            this._previewBlock.removeClass("charactersList-characterPreview-active");
        },
        showInfo: function(withAnimation) {
            if (withAnimation)
                this._infoBlock.stop().slideDown(this._animationTime);
            else
                this._infoBlock.show();
            this._previewBlock.addClass("charactersList-characterPreview-active");
        },
        getItem: function () {
            return this._wrapper;
        },
        characterActionRequestBegin: function () {
            this._actionsLoader.show();
        },
        characterActionRequestComplete: function () {
            this._actionsLoader.hide();
        },
        characterActionRequestSuccess: function (data) {
            this._previewBlock.html(data);
        },
        remove: function () {
            this._wrapper.remove();
            this._actionsLoader.remove();
        }
    }),
    Proxy: Base.extend({
        characterAction: function (url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("characterActionRequestBegin"),
                complete: this.handle("characterActionRequestComplete"),
                success: this.handle("characterActionRequestSuccess")
            });
        }
    })
});