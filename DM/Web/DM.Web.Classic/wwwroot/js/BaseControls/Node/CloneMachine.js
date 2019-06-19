DM.CloneMachine = Base.extend({
    constructor: function(options, view) {
        this._view = view || new DM.CloneMachine.View(options);

        this._callBack = options.callback;
        this._prematureCallback = options.prematureCallback;
    },
    clone: function(nodeId, callback, postInitializationCallback) {
        var clone = this._view.clone(nodeId);
        
        if (callback !== undefined)
            callback.apply(clone);
        if (this._prematureCallback !== undefined)
            this._prematureCallback.apply(clone);

        this._view.reinitizalizeClone(clone, nodeId);

        if (postInitializationCallback !== undefined)
            postInitializationCallback.apply(clone);
        if (this._callBack !== undefined)
            this._callBack.apply(clone);

        return clone;
    },
    clear: function (nodeId) {
        this._view.clear(nodeId);
    },
    getCloneNodesCount: function (nodeId) {
        return this._view.getCloneNodesCount(nodeId);
    },
    setCloneNodesCount: function(nodeId, count) {
        this._view.setCloneNodesCount(nodeId, count);
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this._clearValues = options.clearValues || false;
            this._reinitializeFormValidation = options.reinitializeFormValidation || false;
            this._cascadeClone = options.cascadeClone || false;
            this._cloneNodeModels = options.nodePatterns;
            this._isInForm = options.isInForm || false;

            this.__init();
        },
        __init: function () {
            this._cloneNodeCounts = {};
            this._cloneNodePatterns = {};
            this._cloneNodePrefices = {};
            this._clones = { };
            for (var nodeId in this._cloneNodeModels) {
                this._cloneNodePatterns[nodeId] = this._cloneNodeModels[nodeId].node.clone();
                this._cloneNodeCounts[nodeId] = 0;
                this._cloneNodePrefices[nodeId] = this._cloneNodeModels[nodeId].prefix ||
                                                  this._cloneNodePatterns[nodeId].data("prefix") ||
                                                  "";
                this._clones[nodeId] = [];
            }
        },
        clone: function(nodeId) {
            var clone = this._cloneNodePatterns[nodeId].clone().data('index', this._cloneNodeCounts[nodeId]++);
            this._clones[nodeId].push(clone);
            return clone;
        },
        clear: function (nodeId) {
            var clones = this._clones[nodeId];
            for (var i = 0; i < clones.length; ++i) {
                clones[i].remove();
            }
            this._cloneNodeCounts[nodeId] = 0;
            this._clones[nodeId] = [];
        },
        getCloneNodesCount: function(nodeId) {
            return this._cloneNodeCounts[nodeId];
        },
        reinitizalizeClone: function (node, nodeId) {
            var _this = this;
            var inputs = node.find("input").add(node.filter("input"));
            var labels = node.find("label");
            var validations = node.find("[data-valmsg-for]");

            var prefix = this._cloneNodePrefices[nodeId];
            if (prefix) {
                inputs.each(function() {
                    var input = $(this);
                    var name = input.attr("name");
                    var id = input.attr("id");

                    var newName = name.replace(prefix + "[0]", prefix + "[" + _this._cloneNodeCounts[nodeId] + "]");
                    var newId = id + _this._cloneNodeCounts[nodeId];
                    input.attr({
                        name: newName,
                        id: newId
                    });
                    labels.filter("[for='" + id + "']").attr("for", newId);
                    validations.filter("[data-valmsg-for='" + name + "']").attr("data-valmsg-for", newName);
                });
            }

            if (this._cascadeClone) {
                var cloneNodes = node.find("[data-prefix]");
                cloneNodes.each(function() {
                    var cN = $(this);
                    var cNPrefix = cN.data("prefix");
                    var newPrefix = cNPrefix.replace(prefix + "[0]", prefix + "[" + _this._cloneNodeCounts[nodeId] + "]");
                    cN
                        .attr("data-prefix", newPrefix)
                        .data("prefix", newPrefix);
                });
            }

            if (this._reinitializeFormValidation) {
                var form = node.closest("form");
                form.removeData("validator")
                    .removeData("unobtrusiveValidation")
                    .off("submit.jqueryValidation");
                $.validator.unobtrusive.parse(form);
            }

            if (this._clearValues) {
                inputs.setInputValue("");
            }
        },
        setCloneNodesCount: function(nodeId, count) {
            this._cloneNodeCounts[nodeId] = count;
        }
    })
});