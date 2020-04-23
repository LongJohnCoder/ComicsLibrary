﻿URL = {
    // Search

    getSearchOptions: () =>
        app.apiUrl("search", "getSearchOptions"),

    addToLibrary: () =>
        app.apiUrl("search", "addToLibrary"),

    searchByTitle: (sourceID, title, sortOrder, page) =>
        app.apiUrl("search", "searchByTitle")
            .concat("?", "sourceID", "=", sourceID)
            .concat("&", "title", "=", title)
            .concat("&", "sortOrder", "=", sortOrder)
            .concat("&", "limit", "=", app.maxFetch)
            .concat("&", "page", "=", page),

    getComicsBySourceItemId: (sourceID, sourceItemID, offset) =>
        app.apiUrl("search", "getComics")
            .concat("?", "sourceID", "=", sourceID)
            .concat("&", "sourceItemID", "=", sourceItemID)
            .concat("&", "limit", "=", app.maxFetch)
            .concat("&", "offset", "=", offset),

    // Series

    getNext: () =>
        app.apiUrl("series", "getAllNextUnread"),

    getNextInSeries: (id) =>
        app.apiUrl("series", "getNextUnread")
            .concat("?", "seriesId", "=", id),

    getProgress: (id) =>
        app.apiUrl("series", "getProgress")
            .concat("?", "seriesId", "=", id),

    getBooks: (id, typeId, offset) =>
        app.apiUrl("series", "getBooks")
            .concat("?", "seriesId", "=", id)
            .concat("&", "typeId", "=", typeId)
            .concat("&", "limit", "=", app.maxFetch)
            .concat("&", "offset", "=", offset),

    getSeries: (id, limit) =>
        app.apiUrl("series", "getByID")
            .concat("?", "seriesId", "=", id)
            .concat("&", "limit", "=", (limit ? limit : app.maxFetch)),

    abandonSeries: (id) =>
        app.apiUrl("series", "archive")
            .concat("?", "id", "=", id),

    reinstateSeries: (id) =>
        app.apiUrl("series", "reinstate")
            .concat("?", "id", "=", id),

    removeFromLibrary: (id) =>
        app.apiUrl("series", "remove")
            .concat("?", "id", "=", id),

    getSeriesByStatus: (id) =>
        app.apiUrl("series", "getByStatus")
            .concat("?", "status", "=", id),

    setHomeOption: () =>
        app.apiUrl("series", "setHomeOption"),

    // Books

    markAsRead: (id) =>
        app.apiUrl("books", "markAsRead")
            .concat("?", "id", "=", id),

    markAsUnread: (id) =>
        app.apiUrl("books", "markAsUnread")
            .concat("?", "id", "=", id),

    hideBook: (id) =>
        app.apiUrl("books", "hide")
            .concat("?", "id", "=", id),

    unhideBook: (id) =>
        app.apiUrl("books", "unhide")
            .concat("?", "id", "=", id),


};