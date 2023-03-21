namespace Orc.Memento;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Catel;
using Catel.Data;
using Catel.IoC;
using Catel.Logging;
using Catel.Reflection;

/// <summary>
/// Observer that will observe changes of the the object injected into this observer. Each change will automatically
/// be registered in the <see cref="IMementoService"/>.
/// </summary>
public class ObjectObserver : ObserverBase
{
    /// <summary>
    /// The log.
    /// </summary>
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();
 
    /// <summary>
    /// Collection containing the previous values of the object.
    /// </summary>
    private readonly Dictionary<string, object> _previousPropertyValues = new();

    private INotifyPropertyChanged? _object;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectObserver"/> class.
    /// </summary>
    /// <param name="propertyChanged">The property changed.</param>
    /// <param name="tag">The tag.</param>
    /// <param name="mementoService">The memento service. If <c>null</c>, the service will be retrieved from the <see cref="IServiceLocator"/>.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="propertyChanged"/> is <c>null</c>.</exception>
    public ObjectObserver(INotifyPropertyChanged propertyChanged, object? tag = null, IMementoService? mementoService = null)
        : base(tag, mementoService)
    {
        ArgumentNullException.ThrowIfNull(propertyChanged);

        var propertyChangedType = propertyChanged.GetType();

        Log.Debug("Initializing ObjectObserver for type '{0}'", propertyChangedType.Name);

        _object = propertyChanged;
        _object.PropertyChanged += OnPropertyChanged;

        InitializeDefaultValues(propertyChanged);

        Log.Debug("Initialized ObjectObserver for type '{0}'", propertyChangedType.Name);
    }

    /// <summary>
    /// Called when a property has changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <remarks>
    /// This method must be public because the <see cref="IWeakEventListener"/> is used.
    /// </remarks>
    public void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is null)
        {
            return;
        }

        var propertyName = e.PropertyName;
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return;
        }

        if (sender is ModelBase modelBase)
        {
            if (string.CompareOrdinal(propertyName, "INotifyDataErrorInfo.HasErrors") == 0 ||
                string.CompareOrdinal(propertyName, "INotifyDataWarningInfo.HasWarnings") == 0 ||
                string.CompareOrdinal(propertyName, "IsDirty") == 0)
            {
                return;
            }
        }

        if (ShouldPropertyBeIgnored(sender, propertyName))
        {
            return;
        }

        var oldValue = _previousPropertyValues[propertyName];
        var newValue = PropertyHelper.GetPropertyValue(sender, propertyName);

        // CTL-719: ignore duplicate properties
        if (ObjectHelper.AreEqual(oldValue, newValue))
        {
            return;
        }

        _previousPropertyValues[propertyName] = newValue;

        MementoService.Add(new PropertyChangeUndo(sender, propertyName, oldValue, newValue, Tag));
    }

    /// <summary>
    /// Initializes the default values.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="obj"/> is <c>null</c>.</exception>
    private void InitializeDefaultValues(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var bindingFlags = BindingFlagsHelper.GetFinalBindingFlags(true, false);
        var properties = obj.GetType().GetPropertiesEx(bindingFlags);

        foreach (var property in properties)
        {
            if (!ShouldPropertyBeIgnored(obj, property.Name))
            {
                _previousPropertyValues[property.Name] = PropertyHelper.GetPropertyValue(obj, property.Name);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified property should be ignored.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="propertyName">Name of the property to check.</param>
    /// <returns><c>true</c> if the property should be ignored; otherwise <c>false</c>.</returns>
    private bool ShouldPropertyBeIgnored(object? obj, string? propertyName)
    {
        if (obj is null)
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return true;
        }

        var objectType = obj.GetType();
        var propertyInfo = objectType.GetPropertyEx(propertyName);
        if (propertyInfo is null)
        {
            return true;
        }

        var ignore = propertyInfo.IsDecoratedWithAttribute<IgnoreMementoSupportAttribute>();
        if (ignore)
        {
            Log.Debug("Ignored property '{0}' because it is decorated with the IgnoreMementoSupportAttribute", propertyName);
        }

        return ignore;
    }

    /// <summary>
    /// Clears all the values and unsubscribes any existing change notifications.
    /// </summary>
    public override void CancelSubscription()
    {
        Log.Debug("Canceling property change subscription");

        if (_object is not null)
        {
            _object.PropertyChanged -= OnPropertyChanged;
            _object = null;
        }

        _previousPropertyValues.Clear();

        Log.Debug("Canceled property change subscription");
    }
}
