(function () {
    Base = Base.extend({
        dispose: function () {
            for (var evtNsp in this.__handlers) {
                this.off(evtNsp);
            }

            for (var prop in this) {
                if (this.hasOwnProperty(prop)) {
                    var obj = this[prop];
                    if (obj instanceof DM.PopupBase || obj instanceof DM.Loader) {
                        obj.remove();
                    } else if (obj instanceof Base) {
                        obj.dispose();
                        delete this[prop];
                    } else if (obj instanceof Array && this._tryDisposeArray(obj)) {
                        delete this[prop];
                    }
                }
            }
        },
        _tryDisposeArray: function (array) {
            for (var i = 0; i < array.length; ++i) {
                if (array[i] instanceof Base) {
                    array[i].dispose();
                } else {
                    return false;
                }
            }
            return true;
        },
        _tryDisposeObject: function (obj) {
            for (var prop in obj) {
                if (obj.hasOwnProperty(prop) && obj[prop] instanceof Base) {
                    obj[prop].dispose();
                    delete obj[prop];
                } else {
                    return false;
                }
            }
            return true;
        }
    });
})();