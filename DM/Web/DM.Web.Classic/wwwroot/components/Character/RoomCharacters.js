DM.RoomCharactersControl = Base.extend({
    constructor: function(options, view, proxy, characterControlFactory) {
        this._view = view || new DM.RoomCharactersControl.View(options);
        this._proxy = proxy || new DM.RoomCharactersControl.Proxy(options);
        this._characterControlFactory = characterControlFactory || new DM.RoomCharactersControl.CharacterControlFactory(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("getCharacterDetailsRequest", this._proxy.getCharacterDetails, this._proxy);
        this._proxy.on("getCharacterDetailsRequestBegin", this._view.getCharacterDetailsRequestBegin, this._view);
        this._proxy.on("getCharacterDetailsRequestComplete", this._view.getCharacterDetailsRequestComplete, this._view);
        this._proxy.on("getCharacterDetailsRequestSuccess", this._getCharacterDetailsRequestSuccess, this);
    },
    _getCharacterDetailsRequestSuccess: function (data) {
        if (this._characterControl !== undefined) {
            this._view.removeCharacterLightbox();
        }
        var characterId = this._characterId = this._view.getCharacterDetailsRequestSuccess(data);
        this._characterControl = this._characterControlFactory.create(characterId);
        this._characterControl.on("remove", this._removeCharacter, this);
    },
    _removeCharacter: function () {
        this._view.removeCharacter(this._characterId);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._loader = DM.Loader.create(".js-character-details-link:first");
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            $(document).on("click", ".js-character-details-link", function(evt) {
                evt.preventDefault();
                var $this = $(this);
                _this._loader.bindTo($this);
                _this.trigger("getCharacterDetailsRequest", this.href);
                _this._characterId = $this.data("characterId");
            });
        },
        getCharacterDetailsRequestBegin: function () {
            this._loader.show();
        },
        getCharacterDetailsRequestComplete: function () {
            this._loader.hide();
        },
        getCharacterDetailsRequestSuccess: function (data) {
            if (this._characterDetailsLightbox !== undefined) {
                this._characterDetailsLightbox.remove();
            }
            this._characterDetailsLightbox = DM.Lightbox.create(data);
            this._characterDetailsLightbox.open();
            return this._characterId;
        },
        removeCharacterLightbox: function() {
            if (this._characterDetailsLightbox !== undefined) {
                this._characterDetailsLightbox.remove();
            }
        },
        removeCharacter: function(characterId) {
            $("#CharacterRow_" + characterId).remove();
        }
    }),
    Proxy: Base.extend({
        getCharacterDetails: function (url) {
            $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: this.handle("getCharacterDetailsRequestBegin"),
                complete: this.handle("getCharacterDetailsRequestComplete"),
                success: this.handle("getCharacterDetailsRequestSuccess")
            });
        }
    }),
    CharacterControlFactory: Base.extend({
        create: function (characterId) {
            return new DM.CharacterControl({
                characterId: characterId
            });
        }
    })
});