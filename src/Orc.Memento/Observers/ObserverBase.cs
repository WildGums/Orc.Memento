namespace Orc.Memento;

/// <summary>
/// Base class for all observer classes.
/// </summary>
public abstract class ObserverBase
{
    /// <summary>
    /// The memento service.
    /// </summary>
    private readonly IMementoService _mementoService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObserverBase"/> class.
    /// </summary>
    /// /// <param name="mementoService">The memento service.</param>
    /// <param name="tag">The tag, can be <c>null</c>.</param>
    protected ObserverBase(IMementoService mementoService, object? tag)
    {
        Tag = tag;

        _mementoService = mementoService;
    }

    /// <summary>
    /// Gets or sets the tag.
    /// </summary>
    /// <value>The tag.</value>
    public object? Tag { get; set; }

    /// <summary>
    /// Gets the memento service.
    /// </summary>
    protected IMementoService MementoService
    {
        get { return _mementoService; }
    }

    /// <summary>
    /// Clears all the values and unsubscribes any existing change notifications.
    /// </summary>
    public abstract void CancelSubscription();
}
