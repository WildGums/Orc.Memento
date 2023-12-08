namespace Orc.Memento.Tests;

using System;
using Mocks;
using NUnit.Framework;

public class ActionUndoFacts
{
    #region Nested type: TheActionsThroughMementoServiceMethod
    [TestFixture]
    public class TheActionsThroughMementoServiceMethod
    {
        [TestCase]
        public void CallActions()
        {
            var value = false;
            var mementoService = new MementoService();
            var action = new ActionUndo(this, () => value = true, () => value = false);

            mementoService.Add(action);
            Assert.That(value, Is.False);

            mementoService.Undo();
            Assert.That(value, Is.True);

            mementoService.Redo();
            Assert.That(value, Is.False);
        }

        [TestCase]
        public void SetProperty()
        {
            var instance = new MockCatelModel();
            var action = new PropertyChangeUndo(instance, "Value", "previousValue", "nextValue");
            var mementoService = new MementoService();

            mementoService.Add(action);
            Assert.That(instance.Value, Is.EqualTo(MockCatelModel.ValueProperty.GetDefaultValue()));

            mementoService.Undo();
            Assert.That(instance.Value, Is.EqualTo("previousValue"));

            mementoService.Redo();
            Assert.That(instance.Value, Is.EqualTo("nextValue"));
        }
    }
    #endregion

    #region Nested type: TheConstructor
    [TestFixture]
    public class TheConstructor
    {
        [TestCase]
        public void ThrowsArgumentNullExceptionForNullInstance()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionUndo(null, () => MockModel.Change("test")));
        }

        [TestCase]
        public void ThrowsArgumentNullExceptionForNullUndoMethod()
        {
            var obj = new object();

            Assert.Throws<ArgumentNullException>(() => new ActionUndo(obj, null));
        }
    }
    #endregion

    #region Nested type: TheUndoMethod
    [TestFixture]
    public class TheUndoMethod
    {
        [TestCase]
        public void SetsOldValue()
        {
            var action = new ActionUndo(this, () => MockModel.Change("previousValue"));

            action.Undo();
            Assert.That(MockModel.Name, Is.EqualTo("previousValue"));
        }

        [TestCase]
        public void SetsNewValue()
        {
            var action = new ActionUndo(this, () => MockModel.Change("previousValue"), () => MockModel.Change("nextValue"));

            action.Redo();
            Assert.That(MockModel.Name, Is.EqualTo("nextValue"));
        }

        [TestCase]
        public void SetsOldAndNewValue()
        {
            var action = new ActionUndo(this, () => MockModel.Change("previousValue"), () => MockModel.Change("nextValue"));

            action.Undo();
            Assert.That(MockModel.Name, Is.EqualTo("previousValue"));

            action.Redo();
            Assert.That(MockModel.Name, Is.EqualTo("nextValue"));

            action.Undo();
            Assert.That(MockModel.Name, Is.EqualTo("previousValue"));
        }
    }
    #endregion
}
