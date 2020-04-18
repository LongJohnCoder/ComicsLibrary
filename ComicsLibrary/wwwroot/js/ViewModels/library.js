﻿
library = {
    shelves: [
        { id: 1, title: "Reading", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 2, title: "To Read", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 3, title: "Read", items: ko.observableArray(), fetched: false, selected: ko.observable(false) },
        { id: 4, title: "Archived", items: ko.observableArray(), fetched: false, selected: ko.observable(false) }
    ],
    select: function (data, event) {
        library.setSelected(data.id);
    },
    archiveSeries: function (data, event) {
        update.archiveSeries(data.id);
    },
    reinstateSeries: function (data, event) {
        update.reinstateSeries(data.id);
    },
    deleteSeries: function (data, event) {
        if (!confirm("Delete this series?"))
            return;

        update.deleteSeries(data.id);
    },
    goToSeries: function (data, event) {
        index.loadSeries(data.id);
    },
    onBookStatusUpdated: function (seriesId) {
        API.get(URL.getProgress(seriesId), function (progress) {
            var result = library.find(seriesId);
            result.series.progress = progress;
            library.move(result.series, result.shelf);
        });
    },
    onSeriesArchived: function (seriesId) {
        var result = this.find(seriesId);
        result.series.abandoned = true;
        this.move(result.series, result.shelf);
    },
    onSeriesReinstated: function (seriesId) {
        var result = this.find(seriesId);
        result.series.abandoned = false;
        this.move(result.series, result.shelf);
    },
    onSeriesAdded: function (seriesId) {
        API.get(URL.getSeries(seriesId, 0), function (element) {
            var series = {
                id: element.id,
                title: element.title,
                imageUrl: element.imageUrl,
                abandoned: element.abandoned,
                progress: element.progress,
                unreadIssues: element.unreadIssues,
                totalComics: element.totalComics
            }
            library.move(series, null);
        });
    },
    onSeriesDeleted: function (seriesId) {
        var result = this.find(seriesId);
        result.shelf.items.remove(result.series);
    },
    move: function (item, oldShelf) {
        var newShelf = this.getShelf(series);

        if (oldShelf.id === newShelf.id)
            return;

        if (oldShelf) {
            oldShelf.items.remove(item);
        }

        this.insertItem(newShelf, item);
    },
    getShelf(series) {
        return series.abandoned
            ? 3
            : series.progress === 100
                ? 2
                : series.progress === 0
                    ? 1
                    : 0;
    },
    find: function (seriesId) {
        var foundShelf = null;
        var foundSeries = null;

        $(library.shelves).each(function (i, shelf) {
            if (foundShelf) {
                return false;
            }
            $(shelf.items()).each(function (j, series) {
                if (series.id === seriesId) {
                    foundShelf = shelf;
                    foundSeries = series;
                    return false;
                }
            });
        });

        return {
            shelf: foundShelf,
            series: foundSeries
        };
    },
    insertItem: function (shelfIndex, item) {
        var shelf = library.shelves[shelfIndex];
        for (var i in shelf.items()) {
            if (shelf.items()[i].title > item.title) {
                shelf.items.splice(i, 0, item);
                return false;
            }
        }
        shelf.items.push(item);
    }
};

library.setSelected = function(selectedId){
    var selectedShelf = library.shelves.filter(s => s.id === selectedId)[0];

    $.each(library.shelves, function (index, value) {
        value.selected(false);
    });

    selectedShelf.selected(true);

    if (selectedShelf.fetched)
        return;

    API.get(URL.getSeriesByStatus(selectedId), function (data) {
        selectedShelf.items.removeAll();
        selectedShelf.fetched = true;
        $(data).each(function (index, element) {
            selectedShelf.items.push({
                id: element.id,
                title: element.title,
                imageUrl: element.imageUrl,
                abandoned: element.abandoned,
                progress: element.progress,
                unreadIssues: element.unreadIssues,
                totalComics: element.totalComics
            });
        });
    });
}

library.load = function () {
    library.setSelected(1);
}