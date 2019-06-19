(function($) {
    $(document)
        .on("mousedown.initiateDropdown", ".dds-select", function () {
            var $this = $(this);
            if (!$this.data("initiated")) {
                $this.dropdown();
            }
        })
        .on("mousedown.initiateSuggest focus.initializeSuggest", ".dds-suggest-input", function() {
            var $this = $(this);
            if (!$this.data("initiated")) {
                $this.suggest();
            }
        })
        .on("mousedown.initiateAutocomplete focus.initiateAutocomplete", ".dds-autocomplete-input", function () {
            var $this = $(this);
            if (!$this.data("autocompleteInitiated")) {
                DM.Autocomplete.create($this);
            }
        });
})(jQuery);