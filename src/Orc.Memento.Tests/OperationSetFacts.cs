namespace Orc.Memento.Tests;

using Mocks;
using NUnit.Framework;

public class OperationSetFacts
{
    #region Nested type: TheOperationSetUndoMethod
    [TestFixture]
    public class TheOperationSetUndoMethod
    {
        [TestCase]
        public void UndoTest()
        {
            var mock1 = new MockUndo(true);
            var mock2 = new MockUndo(true);

            var operationSet = new OperationSet(this);
            operationSet.Add(mock1);
            operationSet.Add(mock2);

            operationSet.Undo();

            Assert.That(mock1.UndoCalled, Is.True);
            Assert.That(mock2.UndoCalled, Is.True);
        }

        [TestCase]
        public void UndoOrderTest()
        {
            var finalValue = 0;
            var mock1 = new MockUndo(true);

            var operationSet = new OperationSet(this);
            operationSet.Add(mock1);
            operationSet.Add(new ActionUndo(this, () => finalValue = 1));
            operationSet.Add(new ActionUndo(this, () => finalValue = 2));

            operationSet.Undo();

            Assert.That(mock1.UndoCalled, Is.True);
            Assert.That(finalValue, Is.EqualTo(1));
        }
    }
    #endregion

    #region Nested type: ThePropertyRedoMethod
    [TestFixture]
    public class ThePropertyRedoMethod
    {
        [TestCase]
        public void CanRedoOk()
        {
            var mock1 = new MockUndo(true);
            var mock2 = new MockUndo(true);

            var operationSet = new OperationSet(this);
            operationSet.Add(mock1);
            operationSet.Add(mock2);

            Assert.That(operationSet.CanRedo, Is.True);
        }

        [TestCase]
        public void CanRedoNak()
        {
            var mock1 = new MockUndo(true);
            var mock2 = new MockUndo();

            var operationSet = new OperationSet(this);
            operationSet.Add(mock1);
            operationSet.Add(mock2);

            Assert.That(operationSet.CanRedo, Is.False);
        }

        [TestCase]
        public void RedoTest()
        {
            var mock1 = new MockUndo(true);
            var mock2 = new MockUndo(true);

            var operationSet = new OperationSet(this);
            operationSet.Add(mock1);
            operationSet.Add(mock2);

            operationSet.Redo();
            Assert.That(mock1.RedoCalled, Is.True);
            Assert.That(mock2.RedoCalled, Is.True);
        }
    }
    #endregion
}
