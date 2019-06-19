DM.Suggest = DM.Dropdown.extend({
    constructor: function(options, view) {
        this.base(options, view || new DM.Suggest.View(options));
    },
    __attachEventListeners: function() {
        this.base();
        this._view.on("optionKey", function (option, evt) {
            var btn = evt.keyCode || evt.which;
            if (this.displayed) {
                if (btn === 13) { // enter pressed
                    evt.preventDefault();
                    this._view.selectOption(option);
                    this.trigger("select", option, evt);
                    this.hide();
                } else if (btn === 27 || btn === 9) { // esc pressed
                    this.hide();
                }
            } else if (btn !== 27 && btn !== 9) {
                this.show();
            }
        }, this);
    }
}, {
   View: DM.Dropdown.View.extend({
       __attachDropdownEventListeners: function () {
           var _this = this;
           this._bindItem
               .on("click.showSelect", function (evt) {
                   _this.trigger("inputClick");
               })
               .on("keydown.resolveInputKey", function (evt) {
                   _this.trigger("inputKey", evt);
                   _this.trigger("optionKey", _this._popupItem.find(".dds-option.dds-option-focused .dds-option-link"), evt);
               });
           this._popupItem
               .on("mouseenter", ".dds-option-link:not('.dds-option-disabled')", function () {
                   _this._highlightOption($(this).parent());
               })
               .on("click.selectOption", ".dds-option-link:not('.dds-option-disabled')", function (evt) {
                   var $this = $(this);
                   _this.trigger("optionClick", $this, evt);
               });
       },
       selectOption: function(option) {
           this._bindItem.val(option.data("display"));
           this._bindItem.trigger("input.valueChanged");
       },
       _highlightOption: function (option) {
           this._getOptions().removeClass("dds-option-focused");
           option.addClass("dds-option-focused");
       }
   })
});