DM.FilterPagedListControl = DM.PagedListControl.extend({
    constructor: function(options, view, entityFactory, createEntityControl) {
        this.base(options, view || new DM.FilterPagedListControl.View(options), entityFactory, createEntityControl);
    },
    filter: function(data, pagesCount) {
        this._view.filter(data, pagesCount);
    }
}, {
    View: DM.PagedListControl.View.extend({
        __createPagingControl: function(options) {
            return new DM.FilterPagingControl(options);
        },
        filter: function (data, pagesCount) {
            this._paging.filter(data, pagesCount);
        },
        scrollToTop: function () {
            var top = this._container.offset().top;
            $("#MainWrapper").animate({
                scrollTop: (top > 0 ? "+=" : "-=") + Math.abs(top) + "px"
            }, 200);
        }
    })
});