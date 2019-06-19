DM.CommunityControl = Base.extend({
    constructor: function (options, view) {
        this._view = view || new DM.CommunityControl.View(options);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._filterButton = $("#CommunityFilterButton");
            this._pagedList = new DM.FilterPagedListControl(options);

            this._profileUrlTemplate = options.profileUrlTemplate;
            this._search = DM.Autocomplete.create($("#SearchAutocomplete"));

            this._totalPagesCount = options.totalPages;
            this._totalPagesFilteredCount = options.totalPagesFiltered;

            this.__attachEventListeners();
        },
        __attachEventListeners: function () {
            var _this = this;
            this._filterButton.on("click.filterCommunity", function (evt) {
                _this._toggleFilter();
            });
            this._search.on("selected", this._goToUser, this);
        },
        _goToUser: function (option) {
            var url = this._profileUrlTemplate.replace("__l__", option.data("value"));
            document.location.href = url;
        },
        _toggleFilter: function () {
            var withInactive = this._filterButton.data("withInactive");

            this._pagedList.filter({
                withInactive: withInactive
            }, this._totalPagesCount);
            this._filterButton
                .data("withInactive", !withInactive)
                .swapValue();
        }
    })
});