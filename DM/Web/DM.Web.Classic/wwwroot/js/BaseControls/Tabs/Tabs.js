DM.TabsControl = Base.extend({
    constructor: function(options, view, proxy, historyHandler) {
        this._view = view || new DM.TabsControl.View(options);
        this._proxy = proxy || new DM.TabsControl.Proxy(options);
        this._historyHandler = historyHandler || new DM.TabsControl.HistoryHandler();

        this._uploadedTabs = {};
        this._currentTabId = options.defaultTabId;
        this._defaultTabId = options.defaultTabId || this._view.getDefaultTabId();

        this.__init();
        this.__attachEventListeners();
    },
    __init: function () {
        var uploadedTabIds = this._view.getUploadedTabIds();
        for (var i = 0; i < uploadedTabIds.length; i++) {
            this._uploadedTabs[uploadedTabIds[i]] = true;
        }
    },
    __attachEventListeners: function() {
        this._view.on("tabLink", function(url, tabId, visualUrl, dontPushState) {
            if (!this._uploadedTabs[tabId]) {
                this._proxy.uploadTab(url, tabId, visualUrl);
            } else if (tabId !== this._currentTabId) {
                if (!dontPushState) {
                    this._historyHandler.pushState(visualUrl, tabId);
                }
                this._currentTabId = tabId;
                this.trigger("tabOpened", tabId);
            }
            this._view.showTab(tabId);
        },this);
        this._proxy.on("tabUploadBegin", this._view.tabUploadBegin, this._view);
        this._proxy.on("tabUploadComplete", this._view.tabUploadComplete, this._view);
        this._proxy.on("tabUploadSuccess", function (data, tabId, visualUrl) {
            this._uploadedTabs[tabId] = true;
            this._currentTabId = tabId;
            this._view.tabUploadSuccess(data, tabId);
            this._view.showTab(tabId);
            this._historyHandler.pushState(visualUrl, tabId);
            this.trigger("tabOpened", tabId);
        }, this);

        this._historyHandler.on("popState", function (state) {
            this._view.returnToTab(state ? state.tabId : this._defaultTabId);
        }, this);
    },
    showTab: function (tabId) {
        this._view.showTab(tabId);
    }
}, {
    HistoryHandler: Base.extend({
        constructor: function () {
            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            window.onpopstate = this._handlePopState.bind(this);
        },
        _handlePopState: function(evt) {
            this.trigger("popState", evt.state);
        },
        pushState: function(visualUrl, tabId) {
            if (history.state === null || history.state.tabId !== tabId) {
                history.pushState({ tabId: tabId }, "__tab__" + tabId + "__", visualUrl);
            }
        }
    }),
    View: Base.extend({
        constructor: function (options) {
            options = options || {};

            this._tabLinks = $(options.tabLinkSelector || ".tabLink");
            this._tabBlocks = $(options.tabBlocksSelector || ".tabBlock");

            this._loader = DM.Loader.create(this._tabLinks.first());

            this.__attachEventListeners();
        },
        __attachEventListeners: function() {
            var _this = this;
            this._tabLinks.on("click.openTab", function(evt, dontPushState) {
                evt.preventDefault();
                var url = this.href || this.getAttribute("data-url");
                var tabId = this.getAttribute("data-tab-id");
                var visualUrl = this.getAttribute("data-visual-url");
                _this.trigger("tabLink", url, tabId, visualUrl, dontPushState);
            });
        },
        getUploadedTabIds: function () {
            var result = [];
            this._tabBlocks.each(function() {
                var $this = $(this);
                if ($this.data("tab-uploaded"))
                    result.push($this.attr("id"));
            });
            return result;
        },
        getDefaultTabId: function() {
            var activeLink = this._tabLinks.filter("active:first");
            return activeLink.length > 0
                ? activeLink.data("tabId")
                : null;
        },
        showTab: function (tabId) {
            this._tabBlocks.hide();
            this._getTabBlock(tabId).show();

            this._tabLinks.removeClass("active");
            this._getTabLink(tabId).addClass("active");
        },
        tabUploadBegin: function (tabId) {
            this._loader.bindTo(this._getTabLink(tabId));
            this._loader.show();
        },
        tabUploadComplete: function () {
            this._loader.hide();
        },
        tabUploadSuccess: function (data, tabId) {
            this._getTabBlock(tabId).html(data);
        },
        returnToTab: function (tabId) {
            this._getTabLink(tabId).trigger("click.openTab", [true]);
        },
        _getTabBlock: function (tabId) {
            return this._tabBlocks.filter("#" + tabId);
        },
        _getTabLink: function (tabId) {
            return this._tabLinks.filter("[data-tab-id='" + tabId + "']");
        }
    }),
    Proxy: Base.extend({
        uploadTab: function (url, tabId, visualUrl) {
            if (this._request !== undefined) {
                this._request.abort();
            }

            this._request = $.ajax({
                type: "GET",
                url: url,
                context: this,
                beforeSend: function() { this.trigger("tabUploadBegin", tabId); },
                complete: function () { this.trigger("tabUploadComplete", tabId); },
                success: function (data) {
                    delete this._request;
                    this.trigger("tabUploadSuccess", data, tabId, visualUrl);
                }
            });
        }
    })
});