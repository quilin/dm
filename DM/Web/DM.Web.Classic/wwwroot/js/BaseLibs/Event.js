(function() {
    Base = Base.extend({
        on: function (eventNamespace, handler, context) {
            if (handler instanceof Array) {
                for (var i = 0; i < handler.length; ++i) {
                    this.on(eventNamespace, handler[i], context);
                }
                return this;
            }

            _validateEventNamespace(eventNamespace);
            _validateHandler(handler);

            ((this.__handlers ||
             (this.__handlers = {}))[eventNamespace] ||
             (this.__handlers[eventNamespace] = []))
                .push({ handler: handler, context: context || this });

            return this;
        },
        off: function (eventNamespace) {
            _validateEventNamespace(eventNamespace);
            for (var evtNsp in this.__handlers) {
                if (_matchNamespace(eventNamespace, evtNsp)) {
                    delete this.__handlers[evtNsp];
                }
            }

            return this;
        },
        rebind: function(eventNamespace, handler, context) {
            return this
                .off(eventNamespace)
                .on(eventNamespace, handler, context);
        },
        trigger: function (eventNamespace) {
            _validateEventNamespace(eventNamespace);
            for (var evtNsp in this.__handlers) {
                if (_matchNamespace(eventNamespace, evtNsp)) {
                    var handlerObjs = this.__handlers[evtNsp];
                    for (var i = 0; i < handlerObjs.length; ++i) {
                        var handlerObj = handlerObjs[i];
                        handlerObj.handler.apply(handlerObj.context, [].slice.call(arguments, 1));
                    }
                }
            }
        },
        handle: function(eventNamespace) {
            _validateEventNamespace(eventNamespace);
            return function () {
                for (var evtNsp in this.__handlers) {
                    if (_matchNamespace(eventNamespace, evtNsp)) {
                        var handlerObjs = this.__handlers[evtNsp];
                        for (var i = 0; i < handlerObjs.length; ++i) {
                            var handlerObj = handlerObjs[i];
                            handlerObj.handler.apply(handlerObj.context, arguments);
                        }
                    }
                }
            };
        }
    });

    var _validateEventNamespace = function(eventNamespace) {
            if (typeof eventNamespace !== "string")
                throw new Error("Event namespace should be type of string");
            if (eventNamespace.length === 0)
                throw new Error("Event namespace should not be emtpy string");
        },
        _validateHandler = function(handler) {
            if (handler instanceof Function)
                return;

            throw new Error("Handler should be type of function");
        },
        _matchNamespace = function(eventNamespace, registeredNamespace) {
            return eventNamespace === registeredNamespace ||
                registeredNamespace.indexOf(eventNamespace) === 0;
        }
})();