DM.PagingControl = Base.extend({
    constructor: function (options, view, proxy, historyHandler) {
        this._view = view || new DM.PagingControl.View(options);
        this._proxy = proxy || new DM.PagingControl.Proxy(options);
        this._historyHandler = historyHandler || new DM.PagingControl.HistoryHandler();

        this._currentPage = options.currentPage;
        this._totalPages = options.totalPages;

        this._defaultPage = options.currentPage;
        this._defaultEntity = options.entityNumber;

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._view
            .on("getPageRequest", this.loadPage, this)
            .on("getNextPageRequest", this._loadNextPage, this)
            .on("getPrevPageRequest", this._loadPrevPage, this);
        this._proxy
            .on("getPageRequestBegin", this._view.getPagesRequestBegin, this._view)
            .on("getPageRequestComplete", this._view.getPagesRequestComplete, this._view)
            .on("getPageRequestSuccess", this.handlePageLoaded, this);
        this._historyHandler.on("popState", this._resolveHistoryState, this);
    },
    handlePageLoaded: function (data, url, pushState, afterCreate) {
        if (pushState) {
            this._historyHandler.pushState(url, this._currentPage, this._currentEntity);
        }
        this.trigger("pageLoaded", data, this._currentPage, this._currentEntity, afterCreate);
    },
    reloadPage: function (pageNumber, entityNumber) {
        this._view.resolvePage(pageNumber);
        this._proxy.getPage(entityNumber, false);
        this._currentPage = pageNumber;
        this._currentEntity = entityNumber;
    },
    loadPage: function (pageNumber, entityNumber, pushState, lastPageNumber, afterCreate) {
        this._view.updatePaging(pageNumber, lastPageNumber);
        this._proxy.getPage(entityNumber, pushState, afterCreate);
        this._currentPage = pageNumber;
        this._currentEntity = entityNumber;
    },
    _loadNextPage: function () {
        if (this._currentPage < this._totalPages) {
            this.loadPage(this._currentPage + 1, true);
        }
    },
    _loadPrevPage: function () {
        if (this._currentPage > 1) {
            this.loadPage(this._currentPage - 1, true);
        }
    },
    _resolveHistoryState: function (state) {
        if (state && state.pageNumber !== undefined) {
            this.loadPage(state.pageNumber, state.entityNumber);
        } else if (state === null) {
            this.loadPage(this._defaultPage, this._defaultEntity);
        }
    }
}, {
    HistoryHandler: Base.extend({
        constructor: function () {
            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            window.onpopstate = this._handlePopState.bind(this);
        },
        pushState: function (url, pageNumber, entityNumber) {
            if (history.state === null || history.state.entityNumber !== entityNumber) {
                history.pushState({
                    entityNumber: entityNumber,
                    pageNumber: pageNumber
                }, "__paging__" + entityNumber + "__", url);
            }
        },
        _handlePopState: function (evt) {
            this.trigger("popState", evt.state);
        }
    }),
    View: Base.extend({
        constructor: function (options) {
            this._totalPages = options.totalPages;
            this._currentPage = options.currentPage;
            this._pageSize = options.pageSize;
            this._visualPageUrlTemplate = options.visualPageUrlTemplate || options.pageUrlTemplate;

            this._paginatorContainer = $(options.paginatorContainer);
            this._paginatorHoverContainer = $(options.paginatorHoverContainer);
            this._contentLoader = DM.Loader.create(options.contentContainer);

            this.updatePaging(this._currentPage);

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            $(document)
                .on("click.paging", ".paginator-link", function (evt) {
                    evt.preventDefault();
                    var data = $(this).data();
                    _this.trigger("getPageRequest", data.pageNumber, data.entityNumber, true);
                })
                .on("keydown.paging", function (evt) {
                    var targetTagName = evt.target.tagName.toLowerCase();
                    if (targetTagName !== "textarea" &&
                        targetTagName !== "input" &&
                        evt.ctrlKey) {
                        var btn = evt.keyCode || evt.which;
                        if (btn === 39) { // arrow right
                            _this.trigger("getNextPageRequest");
                        } else if (btn === 37) { // arrow left
                            _this.trigger("getPrevPageRequest");
                        }
                    }
                });
            $("#MainWrapper").on("scroll.paging", function () {
                if (_this._totalPages > 1) {
                    var showHover = _this._paginatorContainer[0].offsetTop + _this._paginatorContainer.height() < $(this).scrollTop();
                    _this._paginatorHoverContainer.toggleClass("shown", showHover);
                }
            });
        },
        updatePaging: function (currentPage, lastPageNumber) {
            this._totalPages = lastPageNumber || Math.max(currentPage, this._totalPages);
            this._currentPage = currentPage;
            var pages = this._getPages(currentPage);
            var mainPagingContainer = this._generatePaging(pages, this._paginatorContainer);
            var hoverPagingContainer = this._generatePaging(pages, this._paginatorHoverContainer);

            var firstLink = mainPagingContainer.find(".paginator-link");
            if (firstLink.length > 0) {
                hoverPagingContainer[0].parentElement.style.left = firstLink[0].offsetLeft + "px";
            }
        },
        resolvePage: function (pageNumber) {
            this._totalPages = Math.min(pageNumber, this._totalPages);
            this.updatePaging(Math.min(this._currentPage, this._totalPages));
        },
        getPagesRequestBegin: function () {
            this._contentLoader.show();
        },
        getPagesRequestComplete: function () {
            this._contentLoader.hide();
        },
        _generatePaging: function (pages, container) {
            container.empty();
            if (pages.length > 1) {
                for (var i = 0; i < pages.length; ++i) {
                    var page = pages[i];
                    $("<a/>", {
                        "html": page.html,
                        "data": {
                            entityNumber: page.entityNumber,
                            pageNumber: page.pageNumber
                        },
                        "href": this._visualPageUrlTemplate.replace("__pn__", page.entityNumber),
                        "class": "paginator-link" + (page.isCurrent ? " active" : "")
                    }).appendTo(container);
                }
            }
            return container;
        },
        _getPages: function (currentPage) {
            var result = [];
            var lowerBound = Math.max(currentPage - 3, 1);
            var upperBound = Math.min(currentPage + 3, this._totalPages);
            for (var i = lowerBound; i <= upperBound; ++i) {
                result.push({
                    html: i,
                    pageNumber: i,
                    entityNumber: (i - 1) * this._pageSize + 1,
                    isCurrent: i === currentPage
                });
            }

            if (result[0].pageNumber !== 1) {
                result[0].pageNumber = 1;
                result[0].entityNumber = 1;
                result[0].html = "<span class=\"base-unselectable iconic\" unselectable=\"on\"></span>";
            }
            if (result[result.length - 1].pageNumber !== this._totalPages) {
                result[result.length - 1].pageNumber = this._totalPages;
                result[result.length - 1].entityNumber = (this._totalPages - 1) * this._pageSize + 1;
                result[result.length - 1].html = "<span class=\"base-unselectable iconic\" unselectable=\"on\"></span>";
            }

            return result;
        }
    }),
    Proxy: Base.extend({
        constructor: function (options) {
            this._pageUrlTemplate = options.pageUrlTemplate;
            this._visualPageUrlTemplate = options.visualPageUrlTemplate || options.pageUrlTemplate;
        },
        __modifyUrl: function (template, entityNumber) {
            return template.replace("__pn__", entityNumber);
        },
        getPage: function (entityNumber, pushState, afterCreate) {
            $.ajax({
                type: "POST",
                url: this.__modifyUrl(this._pageUrlTemplate, entityNumber),
                context: this,
                beforeSend: this.handle("getPageRequestBegin"),
                complete: this.handle("getPageRequestComplete"),
                success: function (data) {
                    this.trigger("getPageRequestSuccess",
                        data,
                        this.__modifyUrl(this._visualPageUrlTemplate, entityNumber),
                        pushState, afterCreate);
                }
            })
        }
    })
});