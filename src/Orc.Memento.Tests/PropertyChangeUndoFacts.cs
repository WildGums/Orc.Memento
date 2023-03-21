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
            Assert.AreEqual("currentValue", obj.MyProperty);
            Assert.AreEqual("MyProperty", propertyChangeUndo.PropertyName);
            Assert.AreEqual(obj, propertyChangeUndo.Target);
            Assert.AreEqual("currentValue", propertyChangeUndo.OldValue);
            Assert.AreEqual("nextValue", propertyChangeUndo.NewValue);
            Assert.AreEqual(true, propertyChangeUndo.CanRedo);
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

            Assert.AreEqual("nextValue", obj.Value);
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

            Assert.AreEqual("previousValue", obj.Value);
        }
    }
}
