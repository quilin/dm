(function ($) {
    var _helpLightbox = DM.Lightbox.create("#__SmartTextAreaTutorial__");

    $(document)
        .on("keydown.smartTextArea", ".smartTextArea", function(evt) {
            if (!evt.ctrlKey || evt.shiftKey) {
                return;
            }

            var btn = evt.keyCode || evt.which;

            if (btn === 66) { // b
                evt.preventDefault();
                insertTag.call(this, { tag: "b" });
            } else if (btn === 73) { // i
                evt.preventDefault();
                insertTag.call(this, { tag: "i" });
            } else if (btn === 85) { // u
                evt.preventDefault();
                insertTag.call(this, { tag: "u" });
            } else if (btn === 83) { // s
                evt.preventDefault();
                insertTag.call(this, { tag: "s" });
            } else if (btn === 76) { // l
                evt.preventDefault();
                insertTag.call(this, { tag: "link", prompt: true, selfattr: true, promptMessage: "Введите URL ссылки:", defaultValue: "" });
            } else if (btn === 81) { // q
                evt.preventDefault();
                insertTag.call(this, { tag: "quote" });
            } else if (btn === 80) { // p
                evt.preventDefault();
                insertTag.call(this, { tag: "img", prompt: true, selfattr: true, promptMessage: "Введите ссылку на изображение:", defaultValue: "", noclosetag: true });
            } else if (btn === 72) { // h
                evt.preventDefault();
                insertTag.call(this, { tag: "spoiler" });
            } else if (btn === 13) {
                evt.preventDefault();
                $(this.form).trigger("submit");
            }
        })
        .on("click.smartTextArea", ".smartTextArea-control", function(evt) {
            var $this = $(this);
            var textArea = $this.parents(".smartTextArea-container").find(".smartTextArea")[0];
            insertTag.call(textArea, $this.data());
        })
        .on("click.showSmartTextAreaHelp", ".smartTextArea-help", function (evt) {
            _helpLightbox.open();
        });

    function insertTag(data) {
        var _this = this,
            tagInfoPromise = generateTag(data);

        tagInfoPromise.done(function (tagInfo) {
            var start = _this.selectionStart;
            var end = _this.selectionEnd;
            var text = _this.value;

            var value = end > 0 && start >= 0 ?
                text.slice(0, start) + tagInfo.openTag + text.slice(start, end) + tagInfo.closeTag + text.slice(end) :
                text + tagInfo.openTag + tagInfo.closeTag;

            _this.value = value;
            _this.selectionStart = start + tagInfo.openTag.length;
            _this.selectionEnd = end + tagInfo.openTag.length;
            $(_this).trigger("input.valueChanged").focus();
        });
    }

    function generateTag(data) {
        var dfd = $.Deferred(),
            attributes = { },
            selfAttribute = "",
            _gen = function() {
                if (data.tag === "ul" || data.tag === "ol") {
                    return {
                        openTag: "[" + data.tag + "][li]",
                        closeTag: "[/li][/" + data.tag + "]"
                    };
                }

                var openTag = "[" + data.tag;
                if (data.selfattr)
                    openTag += "=\"" + selfAttribute + "\"";
                for (var attribute in attributes)
                    openTag += " " + attribute + "=\"" + attributes[attribute] + "\"";
                openTag += "]";

                var closeTag = data.noclosetag ? "" : "[/" + data.tag + "]";

                return {
                    openTag: openTag,
                    closeTag: closeTag
                };
            };

        if (data.prompt) {
            DM.Prompt(data.promptMessage, data.defaultValue)
                .done(function (val) {
                    if (data.selfattr) {
                        selfAttribute = val;
                    } else {
                        attributes[data.promptattr] = val;
                    }
                    dfd.resolve(_gen());
                })
                .fail(function() {
                    dfd.reject();
                });
        } else {
            dfd.resolve(_gen());
        }

        return dfd.promise();
    }
})(jQuery);