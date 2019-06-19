DM.EditCharacterControl = Base.extend({
    constructor: function(options, view, pictureUploadControl) {
        this._view = view || new DM.EditCharacterControl.View(options);
        this._pictureUploadControl = pictureUploadControl || new DM.PictureUploadControl(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._pictureUploadControl.on("uploaded", this._view.updatePictureUrl, this._view);
        this._pictureUploadControl.on("uploadCleared", this._view.clearPictureUrl, this._view);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._pictureUrlInput = $("#PictureUploadRootId");
        },
        updatePictureUrl: function (data) {
            for (var fileName in data.statuses) {
                this._pictureUrlInput.val(data.statuses[fileName].uploadRootId);
            }
        },
        clearPictureUrl: function() {
            this._pictureUrlInput.val("");
        }
    })
});