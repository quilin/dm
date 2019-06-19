(function (arrayPrototype) {
    var _clone = function (arr) {
        var result = [];
        for (var i = 0; i < arr.length; ++i) {
            result.push(arr[i]);
        }
        return result;
    };

    // morphism
    arrayPrototype.where = function (filter) {
        var result = [];
        for (var i = 0; i < this.length; ++i)
            if (filter(this[i]))
                result.push(this[i]);
        return result;
    };
    arrayPrototype.select = function (morphism, context) {
        var result = [];
        for (var i = 0; i < this.length; ++i)
            result.push(morphism.apply(context || this, [this[i], i]));
        return result;
    };
    arrayPrototype.toDictionary = function (keyMorphism, valueMorphism) {
        var result = {};
        for (var i = 0; i < this.length; ++i) {
            result[keyMorphism(this[i], i)] = valueMorphism ? valueMorphism(this[i], i) : this[i];
        }
        return result;
    };
    arrayPrototype.orderBy = function (selector) {
        var clone = _clone(this); // clone so the initial array stays initial
        return clone.sort(function (a, b) {
            return selector(a) - selector(b);
        });
    };
    arrayPrototype.each = function (action, context) {
        for (var i = 0; i < this.length; ++i) {
            action(this[i], i);
        }
    };

    // selectors
    arrayPrototype.first = function (a, b) {
        for (var i = 0; i < this.length; ++i)
            if (b === undefined && a(this[i]) ||
                b !== undefined && this[i][a] === b)
                return this[i];
        throw new Error("Sequence does not contain such element");
    };
    arrayPrototype.firstOrDefault = function (a, b) {
        try {
            return this.first(a, b);
        }
        catch (e) {
            return null;
        }
    };

    // comparators
    arrayPrototype.max = function (morphism) {
        var max = morphism(this[0], 0);
        for (var i = 1; i < this.length; ++i) {
            max = Math.max(max, morphism(this[i], i));
        }
        return max;
    };
    arrayPrototype.min = function (morphism) {
        var min = morphism(this[0], 0);
        for (var i = 1; i < this.length; ++i) {
            min = Math.min(min, morphism(this[i], i));
        }
        return min;
    };

    // booleans
    arrayPrototype.any = function (filter) {
        return this.firstOrDefault(filter) !== null;
    };
    arrayPrototype.all = function (filter) {
        return this.firstOrDefault(function (e) { return !filter(e); }) === null;
    };

    // others
    arrayPrototype.contains = function (value) {
        return this.indexOf(value) >= 0;
    };
    arrayPrototype.separate = function (filter, context) {
        var result = [[], []];
        for (var i = 0; i < this.length; ++i) {
            result[+!filter.apply(context || this, [this[i], i])].push(this[i]);
        }
        return result;
    };
    arrayPrototype.count = function (filter) {
        return this.where(filter).length;
    }
})(Array.prototype);