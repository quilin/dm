DM.ReviewsListControl = Base.extend({
    constructor: function(options, view, proxy, reviewControlFactory) {
        this._view = view || new DM.ReviewsListControl.View(options);
        this._reviewControlFactory = reviewControlFactory || new DM.ReviewsListControl.ReviewControlFactory();

        this._initReviews();
    },
    _initReviews: function() {
        this._reviews = {};
        var reviewIds = this._view.getReviewIds();
        for (var i = 0; i < reviewIds.length; ++i) {
            this._initReview(reviewIds[i]);
        }
    },
    _initReview: function (reviewId) {
        var review = this._reviews[reviewId] = this._reviewControlFactory.create(reviewId);
        review.on("approved", this._reinitReview, this);
        review.on("removed", this._removeReview, this);
    },
    _reinitReview: function(reviewId) {
        this._removeReview(reviewId);
        this._initReview(reviewId);
    },
    _removeReview: function(reviewId) {
        this._reviews[reviewId].dispose();
        delete this._reviews[reviewId];
    }
}, {
    View: Base.extend({
        constructor: function() {
            this._createForm = new DM.FormControl($("#CreateReviewForm")[0]);

            this._createBeginLink = $("#CreateReviewLink");
            this._createCancelLink = $("#CreateReviewCancel");
            this._createReviewMessage = $("#CreateReviewMessage");

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._createBeginLink.on("click.toggle", function (evt) {
                evt.preventDefault();
                _this._toggleForm();
            });
            this._createCancelLink.on("click.toggle", function() {
                _this._toggleForm();
                _this._createForm.resetForm();
            });
            this._createForm.on("requestSuccess", this.createRequestSuccess, this);
        },
        _toggleForm: function() {
            this._createForm.$().toggle();
            this._createBeginLink.toggle();
        },
        getReviewIds: function() {
            return $(".review-wrapper").map(function() {
                return this.getAttribute("data-review-id");
            });
        },
        createRequestSuccess: function() {
            this._toggleForm();
            var _this = this;
            this._createReviewMessage.slideDown(200);
            setTimeout(function() {
                _this._createReviewMessage.slideUp(200);
            }, 10000);
        }
    }),
    ReviewControlFactory: Base.extend({
        create: function(reviewId) {
            return new DM.ReviewControl({
                reviewId: reviewId
            });
        }
    })
});