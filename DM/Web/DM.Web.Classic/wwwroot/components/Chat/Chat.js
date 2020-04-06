DM.ChatControl = Base.extend({
    constructor: function(options, view, proxy) {
        this._view = view || new DM.ChatControl.View(options);
        this._proxy = proxy || new DM.ChatControl.Proxy(options);

        this._requesting = false;

        this.__attachEventListeners();
        this._initLongPoll();
    },
    __attachEventListeners: function () {
        this._view.on("messageCreated", this._loadNewestAfterCreate, this);
        this._view.on("olderEntriesRequest", this._proxy.loadOlderEntries, this._proxy);

        this._proxy.on("loadNewestEntriesRequestBegin", this._loadNewestEntriesRequestBegin, this);
        this._proxy.on("loadNewestEntriesRequestComplete", this._loadNewestEntriesRequestComplete, this);

        this._proxy.on("loadNewestEntriesRequestSuccess", this._loadNewestEntriesRequestSuccess, this);
        this._proxy.on("loadOlderEntriesRequestSuccess", this._view.prependEntries, this._view);
    },
    _loadNewestAfterCreate: function () {
        if (this._requesting) {
            this._proxy.abortNewestEntriesRequest();
        }
        this._loadNewestEntries(true);
    },
    _loadNewestEntries: function (afterCreate) {
        this._proxy.loadNewestEntries({
            fromDate: this._view.getNewestDate()
        }, afterCreate);
    },
    _loadNewestEntriesRequestBegin: function() {
        this._requesting = true;
    },
    _loadNewestEntriesRequestComplete: function () {
        this._requesting = false;
    },
    _loadNewestEntriesRequestSuccess: function (data, afterCreate) {
        this._proxy.abortNewestEntriesRequest();
        this._view.appendEntries(data, afterCreate);
        this._initLongPoll();
    },
    _initLongPoll: function () {
        if (this._longPoll) {
            clearTimeout(this._longPoll);
        }

        var _this = this;
        this._longPoll = setTimeout(function () {
            _this._loadNewestEntries();
        }, 2000);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            var createMessageFormId = options.createMessageFormId || "CreateChatMessageForm";
            this._createMessageForm = new DM.FormControl("#" + createMessageFormId);
            this._container = $("#ChatContainer");
            this._wrapper = $("#ChatMessagesWrapper");
            this._loadOlderEntriesLink = $("#ChatOlderEntriesLink");
            this._noMessagesText = $("#NoPostsMessage");

            this.__attachEventListeners();

            this._scrollToBottom();
        },
        __attachEventListeners: function () {
            this._createMessageForm.on("requestSuccess", function () {
                this.trigger("messageCreated");
                this._createMessageForm.$().find("textarea").val("");
            }, this);

            var _this = this;
            this._loadOlderEntriesLink.on("click.request", function(evt) {
                evt.preventDefault();
                _this.trigger("olderEntriesRequest", {
                    skip: _this._container.find(".chat-message-wrapper").length
                });
            });
        },
        _isScrolledDown: function () {
            return this._container.scrollTop() + this._container.height() - this._wrapper.height() >= 0;
        },
        _scrollToBottom: function () {
            this._container.animate({
                scrollTop: this._container[0].scrollHeight
            });
        },
        getNewestDate: function() {
            return this._container.find(".chat-message-wrapper").last().data("date");
        },
        appendEntries: function(data, afterCreate) {
            if (data) {
                var isScrolledDown = this._isScrolledDown();
                this._noMessagesText.remove();
                this._wrapper.append(data);
                if (afterCreate || isScrolledDown) {
                    this._scrollToBottom();
                }
            }
        },
        prependEntries: function (data) {
            if (data) {
                this._wrapper.prepend(data);
            } else {
                this._loadOlderEntriesLink.remove();
            }
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._loadOlderEntriesUrl = options.loadOlderEntriesUrl;
            this._loadNewestEntriesUrl = options.loadNewestEntriesUrl;

            this._newestEntriesRequest = null;
        },
        abortNewestEntriesRequest: function() {
            this._newestEntriesRequest.abort();
        },
        loadOlderEntries: function(data) {
            $.ajax({
                type: "GET",
                url: this._loadOlderEntriesUrl,
                data: data,
                context: this,
                beforeSend: this.handle("loadOlderEntriesRequestBegin"),
                complete: this.handle("loadOlderEntriesRequestComplete"),
                success: this.handle("loadOlderEntriesRequestSuccess")
            });
        },
        loadNewestEntries: function(data, afterCreate) {
            this._newestEntriesRequest = $.ajax({
                type: "GET",
                url: this._loadNewestEntriesUrl,
                data: data,
                context: this,
                beforeSend: this.handle("loadNewestEntriesRequestBegin"),
                complete: this.handle("loadNewestEntriesRequestComplete"),
                success: function(data) {
                    this.trigger("loadNewestEntriesRequestSuccess", data, afterCreate);
                }
            });
        }
    })
});