namespace Orc.Memento.Tests;

using System;
using Mocks;
using NUnit.Framework;

public class PropertyChangeUndoFacts
{
    [TestFixture]
    public class TheConstructor
    {
        [TestCase]
        public void ThrowsArgumentNullExceptionForNullInstance()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyChangeUndo(null, "PropertyName", null));
        }

        [TestCase]
        public void ThrowsArgumentExceptionForNullPropertyName()
        {
            var obj = new object();

            Assert.Throws<ArgumentException>(() => new PropertyChangeUndo(obj, null, null));
            Assert.Throws<ArgumentException>(() => new PropertyChangeUndo(obj, string.Empty, null));
        }

        [TestCase]
        public void SetsValuesCorrectly()
        {
            var obj = new {MyProperty = "currentValue"};

            var propertyChangeUndo = new PropertyChangeUndo(obj, "MyProperty", "currentValue", "nextValue");
            Assert.That(obj.MyProperty, Is.EqualTo("currentValue"));
            Assert.That(propertyChangeUndo.PropertyName, Is.EqualTo("MyProperty"));
            Assert.That(propertyChangeUndo.Target, Is.EqualTo(obj));
            Assert.That(propertyChangeUndo.OldValue, Is.EqualTo("currentValue"));
            Assert.That(propertyChangeUndo.NewValue, Is.EqualTo("nextValue"));
            Assert.That(propertyChangeUndo.CanRedo, Is.EqualTo(true));
        }
    }

    [TestFixture]
    public class TheRedoMethod
    {
        [TestCase]
        public void SetsNewValue()
        {
            var obj = new MockModel {Value = "currentValue"};

            var propertyChangeUndo = new PropertyChangeUndo(obj, "Value", "previousValue", "nextValue");
            propertyChangeUndo.Redo();

            Assert.That(obj.Value, Is.EqualTo("nextValue"));
        }
    }

    [TestFixture]
    public class TheUndoMethod
    {
        [TestCase]
        public void SetsOldValue()
        {
            var obj = new MockModel {Value = "currentValue"};

            var propertyChangeUndo = new PropertyChangeUndo(obj, "Value", "previousValue", "nextValue");
            propertyChangeUndo.Undo();

            Assert.That(obj.Value, Is.EqualTo("previousValue"));
        }
    }
}
