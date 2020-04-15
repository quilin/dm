DM.FileUploadControl = Base.extend({
    constructor: function (options, view, proxy) {
        this._view = view || new DM.FileUploadControl.View(options);
        this._proxy = proxy || new DM.FileUploadControl.Proxy(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view.on("uploadStart", this._proxy.upload, this._proxy);
        this._proxy.on("uploadProgress", this._view.progress, this._view);
        this._proxy.on("uploadSuccess", this._uploaded, this);
        this._proxy.on("uploadError", this._view.error, this._view);
        
        this._view.on("uploadCancel", this._proxy.cancelUpload, this._proxy);
    },
    _uploaded: function (data) {
        this.trigger("uploaded", data);
        this._view.success(data);
    }
}, {
    View: Base.extend({
        constructor: function ({suffix}) {
            this._input = $(`#UploadFileInput_${suffix}`);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            const _this = this;
            this._input.on("change", function () {
                const data = new FormData();
                const file = this.files[0];
                data.append("files", file);
                _this.trigger("uploadStart", this.getAttribute("data-url"), data, file.name);
            });
        },
        progress: function (percentage) {},
        error: function () {},
        success: function (data) {},
    }),
    Proxy: Base.extend({
        upload: function (url, data, fileName) {
            const _this = this;
            this._uploadRequest = $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                contentType: false,
                processData: false,
                xhr: function () {
                    const _xhr = $.ajaxSettings.xhr();
                    if (_xhr.upload) {
                        _xhr.upload.addEventListener("progress", function (evt) {
                            if (!evt.lengthComputable) return;
                            const percentage = evt.loaded / evt.total * 100;
                            _this.trigger("uploadProgress", percentage, fileName);
                        });
                    }
                    return _xhr;
                },
                success: this.handle("uploadSuccess"),
                error: this.handle("uploadError")
            });
        },
        cancelUpload: function () {
            this._uploadRequest.abort();
        }
    })
});