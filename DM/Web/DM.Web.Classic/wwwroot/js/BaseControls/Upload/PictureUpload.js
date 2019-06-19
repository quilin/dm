DM.PictureUploadControl = Base.extend({
    constructor: function (options, view, proxy, informer) {
        this._view = view || new DM.PictureUploadControl.View(options);
        this._proxy = proxy || new DM.PictureUploadControl.Proxy(options);
        this._informer = informer || DM.NotInformerControl;

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("uploadStart", this._startUpload, this);
        this._proxy.on("getProgressRequestSuccess", this._handleProgress, this);
        this._proxy.on("getNewFormRequestSuccess", this._view.reloadForm, this._view);

        this._view.on("clearUpload", this.handle("uploadCleared"));
        this._view.on("clearUploadRequest", this._proxy.clearUpload, this._proxy);
        this._proxy
            .on("clearUploadRequestSuccess", this.handle("uploadCleared"))
            .on("clearUploadRequestSuccess", this._view.clearPicture, this._view);
    },

    _startUpload: function(uploadId) {
        setTimeout(function () {
            this._proxy.getProgress(uploadId);
        }.bind(this), 1000);
    },
    _handleProgress: function (uploadId, data) {
        for (var fileName in data.statuses) {
            var status = data.statuses[fileName];
            if (status.handled && status.uploaded) {
                this._view.redrawPicture(data);
                this.trigger("uploaded", data);
            } else if (status.handled) {
                this._informer.error(status.message);
            } else {
                this._view.displayProgress(uploadId, status);
            }
        }
        
        if (data.handled) {
            this._proxy.getNewForm();
        } else {
            var _this = this;
            setTimeout(function () {
                _this._proxy.getProgress(uploadId);
            }, 300);
        }
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._pictureContainer = $(".js-picture-upload");
            this._picture = $("#Picture");
            this._clearButton = $("#PictureClearButton");
            this._clearLink = $("#PictureClearLink");

            this._resolveClearLink();

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._pictureContainer.on("change.fileSelected", "form", function (evt) {
                var uploadId = this.getAttribute("data-upload-id");
                if (this.files.value) {
                    this.submit();
                    _this.trigger("uploadStart", uploadId);
                }
            });
            this._clearButton.on("click.request", function(evt) {
                evt.preventDefault();
                _this.clearPicture();
                _this.trigger("clearUpload");
            });
            this._clearLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("clearUploadRequest", this.href);
            });
        },
        displayProgress: function (uploadId, fileInfo) {
            $("#UploadPictureProgress_" + uploadId).show();
            $("#UploadPictureDescription_" + uploadId).hide();
            $("#UploadPictureProgressBar_" + uploadId).css({ width: fileInfo.progress + "%" });
            $("#UploadPictureProgressFile_" + uploadId).text(fileInfo.originalFileName);
        },
        displayError: function (uploadId, fileInfo) {
            $("#UploadPictureProgressBar_" + uploadId).css({ width: fileInfo.progress + "%" }).addClass("error");
            $("#UploadPictureProgressFile_" + uploadId).text(fileInfo.message);
        },
        redrawPicture: function (data) {
            for (var fileName in data.statuses) {
                var status = data.statuses[fileName];
                var newSrc =  status.uploaded ? status.virtualPath : this._picture.data("defaultUrl");
                this._picture.attr("src", newSrc);
                this._resolveClearLink();
                return;
            }
        },
        reloadForm: function (data) {
            this._pictureContainer.find("form").replaceWith(data);
        },
        clearPicture: function () {
            this._picture.attr("src", this._picture.data("defaultUrl"));
            this._resolveClearLink();
        },
        _resolveClearLink: function () {
            var show = this._picture.attr("src") !== this._picture.data("defaultUrl");
            this._clearLink.toggleClass("shown", show);
            this._clearButton.toggleClass("shown", show);
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._getProgressUrl = options.getProgressUrl;
            this._getNewFormUrl = options.getNewFormUrl;
        },
        getNewForm: function () {
            $.ajax({
                type: "GET",
                url: this._getNewFormUrl,
                context: this,
                success: this.handle("getNewFormRequestSuccess")
            });
        },
        getProgress: function (uploadId) {
            $.ajax({
                type: "GET",
                data: { uploadId: uploadId },
                url: this._getProgressUrl,
                context: this,
                success: function (data) {
                    this.trigger("getProgressRequestSuccess", uploadId, data);
                }
            });
        },
        clearUpload: function(url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                success: this.handle("clearUploadRequestSuccess")
            });
        }
    })
});