DM.PopupBase = Base.extend({
        constructor: function(options, view) {
            this._view = view || new DM.PopupBase.View(options);

            this.displayed = false;
            this.fixed = options.fixed || false;
            this._id = DM.PopupRepository.register(this);
        },
        __attachEventListeners: function() {
            this._view.on("hide", this.hide, this);
            this._view.on("windowResize", this.renderPosition, this);
        },
        show: function() {
            if (!this.displayed) {
                /* этот "хак" здесь нужен вот для чего:
                   когда мы привязываем открытие попапа к клику на какую-то ссылку или другому элементу интерфейса,
                   сначала будет вызываться этот метод, однако затем событие начнет всплывать и рано или поздно его перехватит
                   обработчик PopupRepository, который заметит клик вне попапа и попробует скрыть все видимые попапы.
                   таким образом, попап сначала покажется, а потом тут же скроется.
                   есть два решения этой проблемы - одно использовано ниже, такой таймаут заставляет callback функцию
                   переместиться в потоке за текущую задачу (а именно, обработка события, в которую входит и всплытие)
                   таким образом, событие сначала всплывает, а потом обрабатывается отдельно, что есть очень верно.
                   второе решение - вешать на объект ClickEvent StopPropagation, что чревато неочевидным поведением прочих обработчиков
                   и настоятельно не рекомендуется */
                var _this = this;
                window.setTimeout(function () {
                    _this.displayed = true;
                    _this._view.show();
                    _this.trigger("show");
                }, 0);
            }
        },
        hide: function(innerEvent) {
            if (this.displayed) {
                this.displayed = false;
                this._view.hide();
                this.trigger("hide", innerEvent);
            }
        },
        renderPosition: function() {
            if (this.displayed)
                this._view.renderPosition();
        },
        getPopupItem: function() {
            return this._view.getPopupItem();
        },
        getBindItem: function() {
            return this._view.getBindItem();
        },
        remove: function() {
            this._view.remove();
            DM.PopupRepository.pull(this._id);
        }
    }, {
        View: Base.extend({
            constructor: function(options) {
                this._popupItem = options.popupItem;
                this._bindItem = options.bindItem;
                this._position = options.position || {};
                this._automatedPosition = options.position === undefined;
                this._isSticky = options.sticky;
                this._isInLightbox = this._bindItem.parents(".lightbox").length !== 0;

                this._animationTime = 150;
            },
            __init: function () {
                this._popupItem.addClass("popup");

                if (this._isInLightbox) {
                    this._popupItem.appendTo("#LightboxPopupContainer").addClass("lightboxPopup");
                } else {
                    this._popupItem.appendTo("#PopupContainer");
                }

                if (this._isSticky) {
                    this._popupItem.addClass("sticky-popup");
                }
            },
            show: function() {
                this.renderPosition();
                this._popupItem.css("display", "block");
            },
            hide: function() {
                this._popupItem.css("display", "none");
            },
            renderPosition: function() {
                this._popupItem
                    .css(this._countPosition())
                    .css("position", this._isSticky ? "fixed" : "absolute");
            },
            _countPosition: function() {
                var container = this._isInLightbox ? $("#LightboxContainer") : $("#MainWrapper");

                var scrollTop = this._isSticky ? 0 : container.scrollTop();
                var scrollLeft = this._isSticky ? 0 : container.scrollLeft();

                var offset = this._bindItem.offset();
                var offsetTop = offset ? offset.top : 0;
                var offsetLeft = offset ? offset.left : 0;

                if (this._automatedPosition) {
                    var popupHeight = this._popupItem.outerHeight();
                    var containerHeight = container.outerHeight();
                    if (offsetTop + popupHeight > containerHeight - 30) {
                        this._position.bottom = this._bindItem.outerHeight() + 31;
                    }
                }
                var result = {
                    "top": this._position.top !== undefined ?
                        offsetTop + scrollTop + this._position.top :
                        this._position.bottom !== undefined ?
                        offsetTop + scrollTop + this._bindItem.outerHeight() - this._popupItem.outerHeight() - this._position.bottom :
                        offsetTop + scrollTop,
                    "left": this._position.left !== undefined ?
                        offsetLeft + scrollLeft + this._position.left :
                        this._position.right !== undefined ?
                        offsetLeft + scrollLeft + this._bindItem.outerWidth() - this._popupItem.outerWidth() - this._position.right :
                        offsetLeft + scrollLeft
                };

                if (this._automatedPosition && this._position.bottom !== undefined) {
                    delete this._position.bottom;
                }
                return result;
            },
            getPopupItem: function() {
                return this._popupItem;
            },
            getBindItem: function() {
                return this._bindItem;
            },
            remove: function() {
                this._popupItem.remove();
            }
        })
    });