DM.RoomsListControl = Base.extend({
    constructor: function(options, view, proxy, controls) {
        controls = controls || { };

        this._view = view || new DM.RoomsListControl.View(options);
        this._proxy = proxy || new DM.RoomsListControl.Proxy(options);

        this._roomControlFactory = controls.roomControlFactory || new DM.RoomsListControl.RoomControlFactory();
        this._initRoomControls();

        this.__attachEventListeners();
    },
    __attachEventListeners: function() {
        this._view.on("insertAfterRequest", this._proxy.insertAfter, this._proxy);
        this._view.on("insertFirstRequest", this._proxy.insertFirst, this._proxy);
        this._proxy.on("insertRequestBegin", this._view.insertRequestBegin, this._view);
        this._proxy.on("insertRequestComplete", this._view.insertRequestComplete, this._view);
        this._proxy.on("insertRequestError", this._view.insertRequestError, this._view);

        this._view.on("characterAttachRequest", this._proxy.attachCharacter, this._proxy);
        this._proxy.on("attachRequestBegin", this._view.attachRequestBegin, this._view);
        this._proxy.on("attachRequestComplete", this._view.attachRequestComplete, this._view);
        this._proxy.on("attachRequestSuccess", this._view.attachRequestSuccess, this._view);

        this._view.on("createRoomRequest", this._proxy.createRoom, this._proxy);
        this._proxy.on("createRequestBegin", this._view.createRequestBegin, this._view);
        this._proxy.on("createRequestComplete", this._view.createRequestComplete, this._view);
        this._proxy.on("createRequestSuccess", function (data) {
            var roomId = this._view.createRequestSuccess(data);
            this._rooms[roomId] = this._roomControlFactory.create(roomId);
        }, this);

        this._view.on("deleteLinkRequest", this._proxy.deleteLink, this._proxy);
    },
    _initRoomControls: function () {
        var roomIds = this._view.getRoomIds();
        this._rooms = {};
        for (var i = 0; i < roomIds.length; i++) {
            var roomId = roomIds[i];
            this._rooms[roomId] = this._roomControlFactory.create(roomId);
            this._rooms[roomId].on("removed", function() {
                delete this._rooms[roomId];
            }, this);
        }
    }
}, {
    View: Base.extend({
        constructor: function(options) {
            this.roomsList = $(".roomsList-rooms");

            this.characters = $(".roomsList-character");

            this.createRoomForm = $("#CreateRoomForm");
            this.createFormLoader = DM.Loader.create(this.createRoomForm);

            this.__init();
            this.__attachEventListeners();
            this.__createOrUpdateRooms();
        },
        __init: function () {
            $.validator.unobtrusive.parse(this.createRoomForm);

            this.roomsList.sortable({
                revert: 200,
                cancel: ".room-actions, .roomCharacter-deleteLink",
                axis: "y",
                tolerance: "pointer"
            });

            this.characters.draggable({
                helper: "clone",
                revert: "invalid",
                revertDuration: 200
            });
        },
        __attachEventListeners: function() {
            var _this = this;
            this.createRoomForm.on("submit", function(event) {
                event.preventDefault();
                if (_this.createRoomForm.valid()) {
                    _this.trigger("createRoomRequest", this.action, _this.createRoomForm.serialize());
                }
            });

            this.roomsList.on("sortupdate", function(event, ui) {
                var movedRoomNode = ui.item;
                var roomNode = ui.item.prev();

                var data = {
                    movedRoomId: movedRoomNode.data("room-id")
                };
                if (roomNode.length === 0)
                    _this.trigger("insertFirstRequest", data);
                else {
                    data.roomId = roomNode.data("room-id");
                    _this.trigger("insertAfterRequest", data);
                }
            });

            this.roomsList.on("click", ".roomCharacter-deleteLink", function(evt) {
                evt.preventDefault();
                _this.trigger("deleteLinkRequest", this.href);
                $(this).parents(".roomCharacter-wrapper").remove();
            });
        },
        __createOrUpdateRooms: function() {
            var _this = this;

            if (this.rooms)
                this.rooms.droppable("destroy");

            this.rooms = $(".room-wrapper");

            this.rooms.droppable({
                accept: ".roomsList-character",
                hoverClass: "room-wrapper-selected"
            });

            this.rooms.on("drop", function(event, ui) {
                var characterNode = ui.draggable;
                _this.droppedRoomNode = $(event.target);

                var data = {
                    characterId: characterNode.data("character-id"),
                    roomId: _this.droppedRoomNode.data("room-id")
                };

                _this.trigger("characterAttachRequest", data);
            });
        },
        getRoomIds: function () {
            return this.rooms.map(function() {
                return this.getAttribute("data-room-id");
            });
        },
        insertRequestBegin: function() {
            this.roomsList.sortable("disable");
        },
        insertRequestComplete: function() {
            this.roomsList.sortable("enable");
        },
        insertRequestError: function() {
            this.roomsList.sortable("cancel");
        },
        attachRequestBegin: function() {
            this.characters.draggable("disable");
        },
        attachRequestComplete: function() {
            this.characters.draggable("enable");
        },
        attachRequestSuccess: function(data) {
            this.droppedRoomNode.find(".room-characters").append(data);
        },
        createRequestBegin: function() {
            this.createFormLoader.show();
        },
        createRequestComplete: function() {
            this.createFormLoader.hide();
        },
        createRequestSuccess: function (data) {
            var $data = $(data);
            $data.appendTo(this.roomsList);
            this.roomsList.sortable("refresh");
            this.createRoomForm.resetForm();
            this.__createOrUpdateRooms();

            return $data.data("room-id");
        }
    }),
    Proxy: Base.extend({
        constructor: function(options) {
            this._insertRoomAfterUrl = options.insertRoomAfterUrl;
            this._insertFirstRoomUrl = options.insertFirstRoomUrl;
            this._attachCharacterToRoomUrl = options.attachCharacterToRoomUrl;
        },
        _insert: function(data, url) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                beforeSend: this.handle("insertRequestBegin"),
                complete: this.handle("insertRequestComplete"),
                error: this.handle("insertRequestError")
            });
        },
        insertAfter: function(data) {
            this._insert(data, this._insertRoomAfterUrl);
        },
        insertFirst: function(data) {
            this._insert(data, this._insertFirstRoomUrl);
        },
        attachCharacter: function(data) {
            $.ajax({
                type: "POST",
                url: this._attachCharacterToRoomUrl,
                data: data,
                context: this,
                beforeSend: this.handle("attachRequestBegin"),
                complete: this.handle("attachRequestComplete"),
                success: this.handle("attachRequestSuccess")
            });
        },
        createRoom: function(url, data) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                context: this,
                beforeSend: this.handle("createRequestBegin"),
                complete: this.handle("createRequestComplete"),
                success: this.handle("createRequestSuccess")
            });
        },
        deleteLink: function(url) {
            $.ajax({
                type: "POST",
                url: url
            });
        }
    }),
    RoomControlFactory: Base.extend({
        create: function (roomId) {
            return new DM.RoomControl({
                roomId: roomId
            });
        }
    })
});