DM.PictureUploadControl = DM.FileUploadControl.extend({
    constructor: function (options, view, informer) {
        this._informer = informer || DM.NotInformerControl;
        this.base(options,
            view || new DM.PictureUploadControl.View(options));
    }
}, {
    View: DM.FileUploadControl.View.extend({
        constructor: function ({suffix}) {
            this._picture = $(`#Picture_${suffix}`);
            this._progressBar = $(`#UploadProgress_${suffix}`);
            this._progressValue = $(`#UploadProgressBar_${suffix}`);
            this._progressFile = $(`#UploadProgressFile_${suffix}`);
            this._description = $(`#UploadDescription_${suffix}`);
            
            this.base({suffix});
        },
        __attachEventListeners: function () {
            this.base();

            let _this = this;
            this._input.on("change", function (evt) {
                if (this.files && this.files[0]) {
                    let reader = new FileReader();
                    reader.onload = function (readerEvent) {
                        _this._picture.attr("src", readerEvent.target.result);
                    };
                    reader.readAsDataURL(this.files[0]);
                }
            });
        },
        progress: function (percentage, fileName) {
            this._description.hide();

            this._progressBar.show();
            this._progressValue.css({ width: percentage + "%" });
            this._progressFile.text(fileName);
        },
        error: function (error) {
            this._progressValue.addClass("error");
            this._progressFile.text(error);
        },
        success: function (data) {
            this._picture.attr("src", data);
        }
    })
});