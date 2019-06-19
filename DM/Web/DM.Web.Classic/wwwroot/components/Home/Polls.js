DM.PollsControl = function () {
    var PollsControl = Base.extend({
        constructor: function (options, view, pollFactory) {
            this._view = view || new PollsControl.View(options);
            this._pollFactory = pollFactory || new PollsControl.PollFactory(options);

            this._initPolls();
        },
        _initPolls: function () {
            this._polls = {};
            var pollIds = this._view.getPollIds();
            for (var i = 0; i < pollIds.length; ++i) {
                var pollId = pollIds[i];
                this._polls[pollId] = this._pollFactory.create(pollId);
            }
        }
    }, {
        View: Base.extend({
            getPollIds: function () {
                var result = [];
                $(".js-poll").each(function () {
                    result.push(this.getAttribute("data-poll-id"));
                });
                return result;
            }
        }),
        PollFactory: Base.extend({
            create: function (pollId) {
                return new PollControl(pollId);
            }
        })
    });
    var PollControl = Base.extend({
        constructor: function (id, view, proxy) {
            this._view = view || new PollControl.View(id);
            this._proxy = proxy || new PollControl.Proxy();

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            this._view.on("vote", this._proxy.vote, this._proxy);
            this._proxy.on("voteSuccess", this._view.updatePoll, this._view);
        }
    }, {
        View: Base.extend({
            constructor: function (id) {
                this._container = $("#Poll_" + id);

                this.__attachEventListeners();
            },
            __attachEventListeners: function () {
                var _this = this;
                this._container.on("click.vote", ".js-poll-vote-link", function (evt) {
                    evt.preventDefault();
                    _this.trigger("vote", this.href);
                });
            },
            updatePoll: function (data) {
                this._container.replaceWith(data);
            }
        }),
        Proxy: Base.extend({
            vote: function (url) {
                $.ajax({
                    type: "POST",
                    url: url,
                    context: this,
                    success: this.handle("voteSuccess")
                });
            }
        })
    });
    
    return new PollsControl();
}();


