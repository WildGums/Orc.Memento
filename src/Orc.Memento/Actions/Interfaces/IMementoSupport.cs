﻿namespace Orc.Memento;

/// <summary>
/// Interface that describes a single Undo/Redo operation.
/// </summary>
public interface IMementoSupport
{
    /// <summary>
    /// Gets the target.
    /// </summary>
    /// <value>The target.</value>
    object Target { get; }

    /// <summary>
    /// Gets or sets the description, which is option and can be used to display a text to the end-user.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    string Description { get; set; }

    /// <summary>
    /// Gets or sets the tag which can be used to group operations by object.
    /// </summary>
    /// <value>The tag.</value>
    object? Tag { get; set; }

    /// <summary>
    /// Gets a value indicating whether the operation can be "reapplied" after undo.
    /// </summary>
    /// <value><c>true</c> if this instance can redo; otherwise, <c>false</c>.</value>
    bool CanRedo { get; }

    /// <summary>
    /// Method to undo the operation.
    /// </summary>
    void Undo();

    /// <summary>
    /// Method to redo the operation.
    /// </summary>
    void Redo();
}
