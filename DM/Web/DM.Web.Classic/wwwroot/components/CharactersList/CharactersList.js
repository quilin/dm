DM.CharactersListControl = Base.extend({
    constructor: function (options, view, controls) {
        controls = controls || {};

        this._defaultCharacterId = options.defaultCharacterId;

        this._view = view || new DM.CharactersListControl.View(options);
        this._characterControlFactory = controls.characterControlFactory || new DM.CharactersListControl.CharacterControlFactory(options);
        
        this.__init();
        this.__attachEventListeners();
    },
    __init: function () {
        this._characters = {};
        var characterIds = this._view.getCharacterIds();
        for (var i = 0; i < characterIds.length; ++i) {
            var characterId = characterIds[i];
            var character = this._characters[characterIds] = this._characterControlFactory.create(characterId);
            if (characterId === this._defaultCharacterId) {
                this._showCharacterInfo(character);
            }
        }
    },
    _showCharacterInfo: function (character) {
        character.showInfo();
        this._view.scrollToCharacter(character.getItem());
    },
    __attachEventListeners: function () {
        for (var characterId in this._characters) {
            this._characters[characterId].on("remove", function() {
                delete this._characters[characterId];
            }, this);
        }
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._playerCharactersList = $("#PlayerCharactersList");
            this._nonPlayerCharactersList = $("#NonPlayerCharactersList");

            if ($(".tabLink").length > 0) {
                this._tabs = new DM.TabsControl({
                    tabLinkSelector: ".tabLink",
                    tabBlocksSelector: ".tabBlock"
                });
            }
        },
        getCharacterIds: function () {
            var result = [];
            $(".charactersList-characterWrapper").each(function () {
                result.push(this.getAttribute("data-character-id"));
            });
            return result;
        },
        scrollToCharacter: function (item) {
            $("#MainWrapper").animate({
                scrollTop: item.offset().top
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