DM.OrderableTableControl = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.OrderableTableControl.View(options);

        this.__init();
        this.__attachEventListeners();
    },
    __init: function() {

    },
    __attachEventListeners: function() {
        var _this = this;
    }
}, {
    View: Base.extend({
        constructor: function (options, classNames) {
            this._selectors = classNames || DM.OrderableTableControl.View.Selectors;
            this._table = options.table;
            this._tableHeadCells = this._table.find(this._selectors.tableHeadSelector);

            this._ascendingOrder = true;
            this._sortColumn = null;

            this.__init();
            this.__attachEventListeners();
        },
        __init: function () {
            var _this = this;

            this._columnHeads = {};
            this._tableHeadCells.each(function () {
                var columnId = DM.GuidFactory.create();
                var $this = $(this).data("columnId", columnId);

                _this._columnHeads[columnId] = {
                    type: $this.data("sort-type"),
                    number: $this.data("sort-column-number")
                };
            });
        },
        __attachEventListeners: function () {
            var _this = this;
            this._tableHeadCells.on("click.sort", function () {
                _this._sort($(this).data("columnId"));
            });
        },
        _sort: function (columnId) {
            var _this = this;
            var columnHead = this._columnHeads[columnId];

            if (this._sortColumn === columnId) {
                this._ascendingOrder = !this._ascendingOrder;
            } else {
                this._ascendingOrder = true;
                this._sortColumn = columnId;
            }

            var rows = this._table.find(this._selectors.tableRowSelector);
            var values = [];
            rows.each(function() {
                var $this = $(this);
                var cell = $($this.find(_this._selectors.tableCellSelector)[columnHead.number]);
                values.push(cell.data("sort-value"));
            });

            console.info(values);
            values.sort(DM.OrderableTableControl.View.SortingFunctions[columnHead.type]);
            if (this._ascendingOrder)
                values = values.reverse();
            console.info(values);
        }
    }, {
        Selectors: {
            tableHeadSelector: ".charactersList-header .charG-contentWrapper",
            tableRowSelector: ".charactersList-characterRow",
            tableCellSelector: ".charG-contentWrapper"
        },
        SortingFunctions: {
            "String": undefined,
            "Number": function (a, b) {
                return parseFloat(a) - parseFloat(b);
            },
            "Bool": function (a, b) {
                return a - b;
            }
        }
    })
});