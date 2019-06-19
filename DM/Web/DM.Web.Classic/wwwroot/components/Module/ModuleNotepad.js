DM.ModuleNotepadControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.ModuleNotepadControl.View(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {

    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._container = $("#NotepadContainer");
            this._textContainer = $("#NotepadTextContainer");
            this._editLink = $("#NotepadEditLink");
            this._cancelLink = $("#NotepadCancelLink");
            this._editForm = new DM.FormControl("#NotepadEditForm");
            this._editFormText = $("#Notepad");

            this._resolveVisibility();
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._editLink.on("click", function() {
                _this.toggleVisibility();
            });
            this._cancelLink.on("click", function() {
                _this.toggleVisibility();
            });
            this._editForm.on("requestSuccess", function(data) {
                this._resolveVisibility();
                this._textContainer.html(data);
            }, this);
        },
        _resolveVisibility: function () {
            var showForm = this._editFormText.val().length === 0;
            this._container.toggle(!showForm);
            this._editForm.$().toggle(showForm);
            this._cancelLink.toggle(!showForm);
        },
        toggleVisibility: function() {
            this._editForm.$().toggle();
            this._container.toggle();
        }
    })
});