DM.ForumControl = DM.PagedListControl.extend({
    constructor: function (options, view, proxy) {
        this.base(options, view || new DM.ForumControl.View(options), proxy);
    }
}, {
    View: DM.PagedListControl.View.extend({
        constructor: function(options) {
            this._newTopicButton = $("#CreateTopicButton");
            this._createForm = new DM.FormControl("#CreateTopicForm", { nonAjax: true });
            this._cancelCreateButton = $("#CreateTopicCancelButton");
            this._markAsReadForm = new DM.FormControl("#MarkAllAsReadForm", { nonAjax: true });
            this._markAsReadLink = $("#MarkAllAsReadLink");
            this.base(options);
        },
        __attachEventListeners: function () {
            this._newTopicButton.on("click.showForm", function() {
                this._newTopicButton.hide();
                this._createForm.$().show();
            }.bind(this));
            this._cancelCreateButton.on("click.hideForm", function() {
                this._newTopicButton.show();
                this._createForm.$().hide();
            }.bind(this));
            this._markAsReadLink.on("click.markAsRead", function () {
                this._markAsReadForm.submit();
            }.bind(this));
            this.base();
        }
    })
});