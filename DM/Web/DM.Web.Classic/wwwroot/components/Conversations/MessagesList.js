DM.MessagesListControl = Base.extend({
    constructor: function(options, view, proxy, messages) {
        this._view = view || new DM.MessagesListControl.View(options);
        this._proxy = proxy || new DM.MessagesListControl.Proxy(options);
        
        this._messages = messages || new DM.PagedListControl(options);

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("removeSelectedRequest", this._proxy.removeSelected, this._proxy);
        this._proxy.on("removeSelectedRequestBegin", this._view.removeSelectedRequestBegin, this._view);
        this._proxy.on("removeSelectedRequestComplete", this._view.removeSelectedRequestComplete, this._view);
        this._proxy.on("removeSelectedRequestSuccess", this._messages.reloadEntities, this._messages);

        this._view.on("reportSelectedRequest", this._proxy.reportSelected, this._proxy);
        this._proxy.on("reportSelectedRequestBegin", this._view.reportSelectedRequestBegin, this._view);
        this._proxy.on("reportSelectedRequestComplete", this._view.reportSelectedRequestComplete, this._view);
        this._proxy.on("reportSelectedRequestSuccess", this._reportSelectedRequestSuccess, this);

        this._view.on("toggleIgnoreRequest", this._proxy.toggleIgnore, this._proxy);
        this._proxy.on("toggleIgnoreRequestBegin", this._view.toggleIgnoreRequestBegin, this._view);
        this._proxy.on("toggleIgnoreRequestComplete", this._view.toggleIgnoreRequestComplete, this._view);
        this._proxy.on("toggleIgnoreRequestSuccess", this._toggleIgnoreRequestSuccess, this);

        this._messages.on("pageLoaded", this._view.resolveButtons, this._view);
    },
    _reportSelectedRequestSuccess: function(data) {
        this._messages.reloadEntities(data.lastPageNumber);
        this._view.displayReportSuccessMessage(data.canReport);
    },
    _toggleIgnoreRequestSuccess: function (data) {
        this._messages.toggleCreateForm(!data.isIgnored);
        this._view.toggleIgnoreRequestSuccess(data.isIgnored);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._ignoreConversationButton = $("#IgnoreConversation");
            this._ignoreConversationButtonLoader = DM.Loader.create(this._ignoreConversationButton);
            this._ignoredFormMessage = $("#IgnoredFormMessage");

            this._removeSelectedButton = $("#RemoveSelectedMessages");
            this._removeSelectedButtonLoader = DM.Loader.create(this._removeSelectedButton);

            this._reportSelectedButton = $("#ReportSelectedMessages");
            this._reportSelectedButtonLoader = DM.Loader.create(this._reportSelectedButton);
            this._reportSuccessMessage = $("#ReportSuccessMessage");

            this._actionsLink = $("#ShowConversationActionsLink");
            this._actionsBlock = $("#ConversationActions");
            this._actionsPopup = new DM.Popup({
                bindItem: this._actionsLink,
                popupItem: this._actionsBlock,
                position: {
                    right: 0,
                    top: this._actionsLink.height()
                }
            });

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._ignoreConversationButton.on("click.request", function () {
                var $this = $(this);
                if ($this.hasClass("conversation-actions-disabled")) return;
                var url = $this.data("url");
                _this.trigger("toggleIgnoreRequest", url);
            });

            this._removeSelectedButton.on("click.request", function () {
                var $this = $(this);
                if ($this.hasClass("conversation-actions-disabled")) return;
                var url = $this.data("url");
                DM.Confirm().done(function() {
                    _this.trigger("removeSelectedRequest", url, _this._serializeMessageIds());
                });
            });
            this._reportSelectedButton.on("click.request", function () {
                var $this = $(this);
                if ($this.hasClass("conversation-actions-disabled")) return;
                var url = $this.data("url");
                _this.trigger("reportSelectedRequest", url, _this._serializeMessageIds());
            });
            $(document).on("click.toggle", ".message-wrapper", function() {
                $(this).toggleClass("js-message-selected");
                _this.resolveButtons();
            });

            this._actionsLink.on("click.toggleActions", function() {
                _this._actionsPopup.show();
            });
        },
        _getSelectedMessageIds: function () {
            var result = [];
            $(".js-message-selected").each(function() {
                result.push(this.getAttribute("data-message-id"));
            });
            return result;
        },
        _serializeMessageIds: function() {
            var messageIds = this._getSelectedMessageIds();
            var result = {};
            for (var i = 0; i < messageIds.length; ++i) {
                result[i] = messageIds[i];
            }
            return {
                messageIds: result
            };
        },
        resolveButtons: function () {
            var selectedMessages = $(".js-message-selected");
            var messagesToReport = $(".js-message-selected.js-message-to-report");
            this._removeSelectedButton.toggleClass("conversation-actions-disabled", selectedMessages.length === 0);
            this._reportSelectedButton.toggleClass("conversation-actions-disabled", messagesToReport.length === 0);
        },
        removeSelectedRequestBegin: function() {
            this._removeSelectedButtonLoader.show();
        },
        removeSelectedRequestComplete: function () {
            this._removeSelectedButtonLoader.hide();
        },
        reportSelectedRequestBegin: function() {
            this._reportSelectedButtonLoader.show();
        },
        reportSelectedRequestComplete: function () {
            this._reportSelectedButtonLoader.hide();
        },
        displayReportSuccessMessage: function (canReport) {
            this._reportSuccessMessage.slideDown(200);
            var _this = this;
            setTimeout(function() {
                _this._reportSuccessMessage.slideUp();
            }, 5000);

            if (!canReport) {
                this._reportSelectedButton.remove();
                this._reportSelectedButtonLoader.remove();
            }
        },
        toggleIgnoreRequestBegin: function() {
            this._ignoreConversationButtonLoader.show();
        },
        toggleIgnoreRequestComplete: function() {
            this._ignoreConversationButtonLoader.hide();
        },
        toggleIgnoreRequestSuccess: function(isIgnored) {
            this._ignoreConversationButton.swapText();
            this._ignoredFormMessage.toggle(isIgnored);
        }
    }),
    Proxy: Base.extend({
        removeSelected: function(url, data) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                beforeSend: this.handle("removeSelectedRequestBegin"),
                complete: this.handle("removeSelectedRequestComplete"),
                success: this.handle("removeSelectedRequestSuccess")
            });
        },
        reportSelected: function(url, data) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                beforeSend: this.handle("reportSelectedRequestBegin"),
                complete: this.handle("reportSelectedRequestComplete"),
                success: this.handle("reportSelectedRequestSuccess")
            });
        },
        toggleIgnore: function(url) {
            $.ajax({
                type: "POST",
                url: url,
                context: this,
                beforeSend: this.handle("toggleIgnoreRequestBegin"),
                complete: this.handle("toggleIgnoreRequestComplete"),
                success: this.handle("toggleIgnoreRequestSuccess")
            });
        }
    })
});