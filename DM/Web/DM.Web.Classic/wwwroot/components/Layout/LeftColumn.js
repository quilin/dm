DM.ScrollControl = function() {
    var ScrollControl = Base.extend({
        constructor: function(options, view) {
            this._view = view || new ScrollControl.View(options);
        }
    }, {
        View: Base.extend({
            constructor: function(options) {
                this._scrollButton = $("#ScrollButton");
                this._mainWrapper = $("#MainWrapper");
                this._leftColumn = $("#LeftColumn");

                this._hideLink = $("#HideScrollTopButtonLink");

                if (this._hasCookie("__HideScrollTopButton__DM__")) {
                    this._scrollButton.remove();
                }

                this._mainWrapper.focus();

                var _this = this;
                this._leftColumn.find(".toggle-list-link").each(function() {
                    var $this = $(this);
                    var status = $this.data("status");
                    if (_this._hasCookie("__HideLeftMenuModules" + status + "__DM__")) {
                        $this.addClass("hidden");
                        $this.next(".leftMenu-list").hide();
                    }
                });

                this.__attachEventListeners();
            },
            __attachEventListeners: function() {
                var _this = this;
                this._scrollButton.on("click.scrollToTop", function() {
                    _this._mainWrapper.animate({ scrollTop: 0 }, 200);
                });
                this._mainWrapper.on("scroll", function() {
                    _this._resolveButton();
                });
                this._hideLink.on("click.hideLink", function(evt) {
                    evt.stopPropagation();
                    _this._setCookie("__HideScrollTopButton__DM__");
                    _this._scrollButton.remove();
                });
                this._leftColumn.on("click.toggleList", ".toggle-list-link", function() {
                    var $this = $(this);
                    $this.toggleClass("hidden");
                    $this.next(".leftMenu-list").slideToggle(200);
                    var status = $this.data("status");
                    if ($this.hasClass("hidden")) {
                        _this._setCookie("__HideLeftMenuModules" + status + "__DM__");
                    } else {
                        _this._removeCookie("__HideLeftMenuModules" + status + "__DM__");
                    }
                });
            },
            _resolveButton: function() {
                this._scrollButton.toggle(this._leftColumn.offset().top + this._leftColumn.height() < 0);
            },

            _hasCookie: function(name) {
                return document.cookie.indexOf(name) !== -1;
            },
            _setCookie: function(name) {
                document.cookie = name + "=" + name + ";path=/;expires=" + new Date(new Date().getFullYear() + 10, 1, 1).toGMTString();
            },
            _removeCookie: function(name) {
                document.cookie = name + "=" + name + ";path=/;expires=" + new Date(0).toGMTString();
            }
        })
    });

    return new ScrollControl();
}();