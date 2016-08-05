# Orc.Memento

[![Join the chat at https://gitter.im/WildGums/Orc.Memento](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/WildGums/Orc.Memento?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![License](https://img.shields.io/github/license/WildGums/Orc.Memento.svg)
![NuGet downloads](https://img.shields.io/nuget/dt/Orc.Memento.svg)
![Version](https://img.shields.io/nuget/v/Orc.Memento.svg)
![Pre-release version](https://img.shields.io/nuget/vpre/Orc.Memento.svg)

# Introduction

Lots of real world applications need to implement undo/redo. However, most applications written in MVVM lack this feature because it is very hard to implement. Luckily, Catel solves this issue by introducing the `IMementoService`. The `IMementoService` is a service that allows a developer to register custom actions that should be undone. A few actions you can think of:

* Property change of a model
* Item is added or removed to/from a collection
* A method is executed

One way to introduce the memento pattern is by creating a copy of the whole memory at each step (yes, some people actually do this), but in Catel it is done a bit smarter. For each possible action type, there is an implementation of the `UndoBase`. This way, each action will know by itself how to undo or redo. Catel offers the following default implementations:

* PropertyChangeUndo
* CollectionChangeUndo
* ActionUndo

If there are more actions supported, it is possible to write a custom `UndoBase` implementation and add the specific action to the `IMementoService`. It will automatically be added to the undo/redo action stack.

## Undo and redo support

The `IMementoService` supports both undo and redo actions. This means that an action that is undo-ed by a call to the `Undo` method, it is automatically added to the redo stack when redo is supported.

To undo an action, use the code below:

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.Undo();

It is possible to check whether it is possible to undo actions by using the `CanUndo` property. This check is not required since the `Undo` method will also check this internally.

To redo an action, use the code below:

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.Redo();

It is possible to check whether it is possible to redo actions by using the `CanRedo` property. This check is not required since the `Redo` method will also check this internally.

## Grouping actions in batches

The `MementoService` automatically wraps all actions in batches. Because each action is treated as a batch, it is easy to begin a batch and add several actions to a single batch. Below is the code to create a batch:

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.BeginBatch("Batch title", "Batch description");
	 
	// All actions added to the memento service are added to the specified batch

*Note that the Title and Description are optional. They are however a great way to represent the batches in the user interface*

A batch can be ended in several ways:

1. A call to EndBatch
2. A call to BeginBatch

As soon as a batch is ended by one of the ways described above, it will be added to the undo stack.

## Ignoring support for memento

Ignoring specific properties or methods for the IMementoService is very easy. Just decorate them with the IgnoreMementoSupportAttribute as shown below:

	[IgnoreMementoSupport]
	public string IgnoredProperty { get; set; }


# Memento and properties

Adding the ability to undo and redo property changes on an object is very simple using the `PropertyChangeUndo` class. This can be done either automatically or manually.

## Handling property changes automatically

When an object implements the `INotifyPropertyChanged` interface, it is possible to register the object. The `IMementoService` will fully take care of any property changes by the object and add these automatically to the undo/redo stack. Internally, the service will create an instance of the `ObjectObserver` which will register the changes in the `IMementoService`.

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.RegisterObject(myObject);

## Handling property changes manually

When an object does not support the `INotifyPropertyChanged` interface or you want more control, it is possible to instantiate the `PropertyChangeUndo` yourself. See the example below:

	public string Name
	{
	    get { return _name; }
	    set
	    {
	        object oldValue = _name;
	        object newValue = value;
	 
	        _name = value;
	 
	        RaisePropertyChanged("Name");
	 
	        var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	        mementoService.Add(new PropertyChangeUndo(this, "Name", oldValue, newValue)); 
	    }
	}

## Removing an object and its actions

When a model goes out of scope, it is important that the `IMementoService` does not keep it in memory and keeps undoing the changes. Therefore, one should also unregister the object:

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.UnregisterObject(myObject);

*Note that unregistering an object will both cancel change notifications and remove the actions that belong to this object from the undo/redo stack*


# Memento and collections

Adding the ability to undo and redo collection changes on a collection is very simple using the `CollectionChangeUndo` class. This can be done either automatically or manually.

## Handling collection changes automatically

When a collection implements the `INotifyCollectionChanged` interface, it is possible to register the collection. The `IMementoService` will fully take care of any collection changes by the collection and add these automatically to the undo/redo stack. Internally, the service will create an instance of the `CollectionObserver` which will register the changes in the `IMementoService`.

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.RegisterCollection(myCollection);

## Handling collection changes manually

When an object does not support the `INotifyCollectionChanged` interface or you want more control, it is possible to instantiate the `CollectionChangeUndo` yourself. See the example below:

	public void AddPerson(IPerson person)
	{
	    var newIndex = _internalCollection.Add(person);
	     
	    var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	    mementoService.Add(new CollectionChangeUndo(_internalCollection, CollectionChangeType.Add, -1, newIndex, null, item)); 
	}

*Note that all actions should be implemented, such as adding, replacing, removing and resetting to fully support undo/redo*

## Removing a collection and its actions

When a collection goes out of scope, it is important that the `IMementoService` does not keep it in memory and keeps undoing the changes. Therefore, one should also unregister the collection:

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.UnregisterCollection(myCollection);

*Note that unregistering a collection will both cancel change notifications and remove the actions that belong to this collection from the undo/redo stack*


# Memento and methods

Adding the ability to undo and redo methods is the most complex, because this cannot be done automatically. However, it is possible to use the `ActionUndo` class to make it as easy as possible.

## Handling methods manually

An action can come in two flavors. One with only undo support, and one with redo support. It is always recommended to implement the one with support for redo, but the choice is always yours. For this example, let's assume a simple class that will increase a value (for which we are building undo/redo support):

	public class SpecialNumberContainer()
	{
	    private int _number = 5;
	     
	    public int Number { get { return _number; } }
	     
	    public int IncreaseNumber()
	    {
	        _number++;
	    }
	}

As you can see in the example, it is not possible to use the `PropertyChangeUndo` because the property has no setter and no change notification. So, we will create custom actions for undo/redo.

First, the class with only undo support:

	public class SpecialNumberContainer()
	{
	    private int _number = 5;
	 
	    public int Number { get { return _number; } }
	     
	    public int IncreaseNumber()
	    {
	        _number++;
	         
	        var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	        mementoService.Add(new ActionUndo(this, () => _number--)); 
	    }
	}

The code above will add a new action to the undo stack every time the `IncreaseNumber` method is called. Then, it will not add it to the redo stack because redo is not possible (we haven't provided a redo action).

Below is the same class, but now with redo support:

	public class SpecialNumberContainer()
	{
	    private int _number = 5;
	 
	    public int Number { get { return _number; } }
	     
	    public int IncreaseNumber()
	    {
	        _number++;
	         
	        var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	        mementoService.Add(new ActionUndo(this, () => _number--, () => _number++)); 
	    }
	}

The code above will add a new action to the undo stack every time the `IncreaseNumber` method is called. Then, when an action is undo-ed, the action is added to the redo stack and it is possible to redo the action because the redo action was provided as well.

## Removing the actions from the undo/redo stack

When an action no longer has to be in the undo/redo stack of the `IMementoService`, one should call the `Clear` method with the instance of the method as shown in the sample below:

	var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
	mementoService.Clear(myInstanceContainingTheMethod);