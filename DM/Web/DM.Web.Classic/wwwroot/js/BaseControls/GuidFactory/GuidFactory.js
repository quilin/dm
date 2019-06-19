DM.GuidFactory = {
    empty: "00000000-0000-0000-0000-000000000000",
    create: function () {
        return this._s4() + this._s4() + "-" + this._s4() + "-" +
               this._s4() + "-" + this._s4() + "-" + this._s4() +
               this._s4() + this._s4();
    },
    _s4: function () {
        return Math.floor((1 + Math.random()) * 0x10000)
                   .toString(16)
                   .substring(1);
    }
};