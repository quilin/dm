DM.FilterPagingControl = DM.PagingControl.extend({
    constructor: function (options, view, proxy) {
        this.base(options, view, proxy || new DM.FilterPagingControl.Proxy(options));
    },
    filter: function(data, totalPagesCount) {
        this._proxy.setData(data);
        this.loadPage(1, 1, true, totalPagesCount);
    }
}, {
    Proxy: DM.PagingControl.Proxy.extend({
        constructor: function(options) {
            this.base(options);
            this._data = options.filterData;
        },
        setData: function(filterData) {
            this._data = filterData;
        },
        getPage: function (entityNumber, pushState) {
            $.ajax({
                type: "POST",
                url: this.__modifyUrl(this._pageUrlTemplate, entityNumber),
                data: this._data,
                context: this,
                beforeSend: this.handle("getPageRequestBegin"),
                complete: this.handle("getPageRequestComplete"),
                success: function(data) {
                    this.trigger("getPageRequestSuccess", data, this.__modifyUrl(this._visualPageUrlTemplate, entityNumber), pushState);
                }
            });
        }
    })
});