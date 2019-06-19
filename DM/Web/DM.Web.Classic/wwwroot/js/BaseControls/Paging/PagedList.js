DM.PagedListControl = Base.extend({
    constructor: function(options, view, entityFactory, createEntityControl) {
        this._view = view || new DM.PagedListControl.View(options);
        this._entityFactory = entityFactory || new DM.PagedListControl.EntityFactory(options);
        this._createEntityControl = createEntityControl || new DM.CreatePagedListEntityControl();

        this._entities = {};
        this._currentPage = options.currentPage;
        this._pageSize = options.pageSize;
        this._initEntities();

        this._highlightEntity(options.entityNumber);

        this.__attachEventListeners();
    },
    __attachEventListeners: function () {
        this._createEntityControl.on("created", function (pageNumber) {
            this._view.loadPage(pageNumber, undefined, true);
        }, this);
        this._view.on("pageLoaded", this._loadEntities, this);
    },
    _initEntities: function () {
        var entityIds = this._view.getEntityIds();
        for (var i = 0; i < entityIds.length; ++i) {
            this._initEntity(entityIds[i]);
        }
    },
    _initEntity: function (entityId) {
        var entity = this._entities[entityId] = this._entityFactory.create(entityId);
        entity.on("removed", this.reloadEntities, this);
        entity.on("edited", this._processEditedEntity, this);
        entity.on("editInitiated", this._resolveEditing, this);
    },
    _loadEntities: function (data, pageNumber, entityNumber, afterCreate) {
        var loadedEntities = this._view.loadEntities(data);
        this._initEntities(loadedEntities);
        this._currentPage = pageNumber;
        if (afterCreate) {
            this._highlightEntity(loadedEntities.length);
        } else {
            this._highlightEntity(entityNumber);
        }
        this.trigger("pageLoaded");
    },
    _highlightEntity: function (entityNumber) {
        if (entityNumber === 1) {
            this._view.scrollToTop();
        } else {
            var number = entityNumber % this._pageSize;
            var entityId = this._view.getEntityIdByNumber(number);
            if (entityId) {
                this._entities[entityId].highlight();
            }
        }
    },
    reloadEntities: function (lastPageNumber) {
        var parsedLastPageNumber = parseInt(lastPageNumber);
        this._view.loadPage(Math.min(this._currentPage, parsedLastPageNumber), parsedLastPageNumber);
    },
    _resolveEditing: function (editedEntityId) {
        for (var entityId in this._entities) {
            if (entityId !== editedEntityId) {
                this._entities[entityId].cancelEdit();
            }
        }
    },
    _processEditedEntity: function (entityId) {
        this.trigger("editedEntity", entityId);
        this._initEntity(entityId);
    },
    toggleCreateForm: function(show) {
        this._createEntityControl.toggle(show);
    }
}, {
    View: Base.extend({
        constructor: function (options) {
            this._container = $("#Container");

            this._paging = this.__createPagingControl($.extend(options, {
                paginatorContainer: "#Paginator",
                paginatorHoverContainer: "#PaginatorHover",
                contentContainer: this._container
            }));

            this._pageSize = options.pageSize;

            this.__attachEventListeners();
        },
        __createPagingControl: function (options) {
            return new DM.PagingControl(options);
        },
        __attachEventListeners: function () {
            this._paging.on("pageLoaded", this.handle("pageLoaded"), this);
        },
        getEntityIds: function (data) {
            var blocks = data ? $(data) : this._container.find(".js-entity-wrapper");
            return blocks.map(function() {
                return this.getAttribute("data-entity-id");
            });
        },
        getEntityIdByNumber: function (number) {
            var entities = this._container.find(".js-entity-wrapper");
            return entities.length === 0 ? undefined : entities.get(number - 1).getAttribute("data-entity-id");
        },
        loadPage: function (pageNumber, lastPageNumber, afterCreate) {
            pageNumber = parseInt(pageNumber);
            this._paging.loadPage(pageNumber, (pageNumber - 1) * this._pageSize + 1, false, lastPageNumber, afterCreate);
        },
        reloadPage: function (pageNumber) {
            pageNumber = parseInt(pageNumber);
            this._paging.reloadPage(pageNumber, (pageNumber - 1) * this._pageSize + 1);
        },
        loadEntities: function (data) {
            this._container.html(data);
            return this._container.children();
        },
        scrollToTop: function() {
            $("#MainWrapper").animate({
                scrollTop: 0
            }, 200);
        }
    }),
    EntityFactory: Base.extend({
        constructor: function (options) {
            this._withWarnings = options.withWarnings;
        },
        create: function (entityId) {
            return new DM.PagedListEntityControl({
                entityId: entityId,
                withWarnings: this._withWarnings
            });
        }
    })
});