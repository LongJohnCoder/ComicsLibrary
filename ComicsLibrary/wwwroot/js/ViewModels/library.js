﻿
library = {
    shelves: ko.observableArray(),
    loaded: false,
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
        update.deleteSeries(data.id);
    },
    goToSeries: function (data, event) {
        index.loadSeries(data.id);
    },
    onBookStatusUpdated: function (seriesId) {
        if (!library.loaded)
            return;

        var result = library.find(seriesId);
        if (!result.series) {
            library.onSeriesAdded(seriesId);
        }
        else {
            API.get(URL.getProgress(seriesId), function (progress) {
                result.series.progress = progress;
                library.move(result.series, result.shelf);
            });
        }
    },
    onSeriesArchived: function (seriesId) {
        if (!this.loaded)
            return;

        var result = this.find(seriesId);
        if (!result.series) {
            library.onSeriesAdded(seriesId);
        }
        else {
            result.series.abandoned = true;
            library.move(result.series, result.shelf);
        }
    },
    onSeriesReinstated: function (seriesId) {
        if (!this.loaded)
            return;

        var result = this.find(seriesId);
        if (!result.series) {
            library.onSeriesAdded(seriesId);
        }
        else {
            result.series.abandoned = false;
            library.move(result.series, result.shelf);
        }
    },
    onSeriesAdded: function (seriesId) {
        if (!this.loaded)
            return;

        API.get(URL.getLibrarySeries(seriesId, 0), function (element) {
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
        if (!this.loaded)
            return;

        var result = this.find(seriesId);
        if (!result.series)
            return;
        result.shelf.items.remove(result.series);
    },
    move: function (item, oldShelf) {
        var newShelfId = library.getShelf(item);
        if (oldShelf) {
            if (oldShelf.id === newShelfId) {
                return;
            }   

            oldShelf.items.remove(item);
        }

        this.insertItem(newShelfId, item);
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

        $(library.shelves()).each(function (i, shelf) {
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
        if (!this.loaded)
            return;

        var shelf = library.shelves()[shelfIndex];

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
    var selectedShelf = library.shelves().filter(s => s.id === selectedId)[0];

    $.each(library.shelves(), function (index, value) {
        value.selected(false);
    });

    selectedShelf.selected(true);
}

library.load = function () {
    API.get(URL.getLibraryShelves(), function (data) {

        $(data).each(function (index, element) {
            var shelf = {
                id: element.statusId,
                title: element.status,
                items: ko.observableArray(),
                selected: ko.observable(false)
            };

            library.shelves.push(shelf);

            $(element.series).each(function (j, series) {
                shelf.items.push({
                    id: series.id,
                    title: series.title,
                    imageUrl: series.imageUrl,
                    abandoned: series.archived,
                    progress: series.progress,
                    unreadIssues: series.unreadBooks,
                    totalComics: series.totalBooks
                });
            });
        });

        library.loaded = true;

        library.setSelected(0);
    });
}