namespace Orc.Memento;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Catel;
using Catel.Logging;
using Microsoft.Extensions.Logging;

/// <summary>
/// This class provides a simple <see cref="INotifyCollectionChanged"/> observer that will add undo/redo support to a 
/// collection class automatically by monitoring the collection changed events.
/// </summary>
public class CollectionObserver : ObserverBase
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(CollectionObserver));

    /// <summary>
    /// The collection.
    /// </summary>
    private INotifyCollectionChanged? _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionObserver"/> class.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="mementoService">The memento service.</param>
    /// <param name="tag">The tag.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="collection"/> is <c>null</c>.</exception>
    public CollectionObserver(INotifyCollectionChanged collection, IMementoService mementoService, object? tag)
        : base(mementoService, tag)
    {
        ArgumentNullException.ThrowIfNull(collection);

        _collection = collection;
        _collection.CollectionChanged += OnCollectionChanged;
    }

    /// <summary>
    /// This is invoked when the collection changes.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
    /// <remarks>
    /// This method must be public because the <see cref="IWeakEventListener"/> is used.
    /// </remarks>
    public void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not IList collection)
        {
            return;
        }

        Logger.LogDebug("Automatically tracking change '{0}' of collection", e.Action);

        var undoList = new List<CollectionChangeUndo>();

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is not null)
                {
                    undoList.AddRange(e.NewItems.Cast<object>().Select((item, i) => new CollectionChangeUndo(collection, CollectionChangeType.Add, -1, e.NewStartingIndex + i, null, item, Tag)));
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is not null)
                {
                    undoList.AddRange(e.OldItems.Cast<object>().Select((item, i) => new CollectionChangeUndo(collection, CollectionChangeType.Remove, e.OldStartingIndex + i, -1, item, null, Tag)));
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.NewItems is not null && e.OldItems is not null)
                {
                    undoList.Add(new CollectionChangeUndo(collection, CollectionChangeType.Replace, e.OldStartingIndex, e.NewStartingIndex, e.OldItems[0], e.NewItems[0], Tag));
                }
                break;

            case NotifyCollectionChangedAction.Move:
                if (e.NewItems is not null)
                {
                    undoList.Add(new CollectionChangeUndo(collection, CollectionChangeType.Move, e.OldStartingIndex, e.NewStartingIndex, e.NewItems[0], e.NewItems[0], Tag));
                }
                break;
        }

        foreach (var operation in undoList)
        {
            MementoService.Add(operation);
        }

        Logger.LogDebug("Automatically tracked change '{0}' of collection", e.Action);
    }

    /// <summary>
    /// Clears all the values and unsubscribes any existing change notifications.
    /// </summary>
    public override void CancelSubscription()
    {
        Logger.LogDebug("Canceling collection change subscription");

        if (_collection is not null)
        {
            _collection.CollectionChanged -= OnCollectionChanged;
            _collection = null;
        }

        Logger.LogDebug("Canceled collection change subscription");
    }
}
