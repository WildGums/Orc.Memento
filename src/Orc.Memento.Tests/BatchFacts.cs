namespace Orc.Memento.Tests;

using NUnit.Framework;

public class BatchFacts
{
    #region Nested type: TheActionCountProperty
    [TestFixture]
    public class TheActionCountProperty
    {
        [TestCase]
        public void ReturnsZeroForEmptyBatch()
        {
            var batch = new Batch();

            Assert.That(batch.ActionCount, Is.EqualTo(0));
        }

        [TestCase]
        public void ReturnsOneForBatchWithOneAction()
        {
            var model = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new PropertyChangeUndo(model, "Value", model.Value));

            Assert.That(batch.ActionCount, Is.EqualTo(1));
        }
    }
    #endregion

    #region Nested type: TheCanRedoProperty
    [TestFixture]
    public class TheCanRedoProperty
    {
        [TestCase]
        public void IsFalseWhenNoActionsCanRedo()
        {
            var model1 = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new ActionUndo(model1, () => model1.Value = "Value"));

            Assert.That(batch.CanRedo, Is.False);
        }

        [TestCase]
        public void IsTrueWhenAtLeastOneActionCanRedo()
        {
            var model1 = new Mocks.MockModel();
            var model2 = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new PropertyChangeUndo(model1, "Value", model1.Value));
            batch.AddAction(new PropertyChangeUndo(model2, "Value", model2.Value));

            Assert.That(batch.CanRedo, Is.True);
        }
    }
    #endregion

    #region Nested type: TheIsEmptyBatchProperty
    [TestFixture]
    public class TheIsEmptyBatchProperty
    {
        [TestCase]
        public void ReturnsTrueForEmptyBatch()
        {
            var batch = new Batch();

            Assert.That(batch.IsEmptyBatch, Is.True);
        }

        [TestCase]
        public void ReturnsFalseForBatchWithOneAction()
        {
            var model = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new PropertyChangeUndo(model, "Value", model.Value));

            Assert.That(batch.IsEmptyBatch, Is.False);
        }

        [TestCase]
        public void ReturnsFalseForBatchWithMultipleActions()
        {
            var model1 = new Mocks.MockModel();
            var model2 = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new PropertyChangeUndo(model1, "Value", model1.Value));
            batch.AddAction(new PropertyChangeUndo(model2, "Value", model2.Value));

            Assert.That(batch.IsEmptyBatch, Is.False);
        }
    }
    #endregion

    #region Nested type: TheIsSingleActionBatchProperty
    [TestFixture]
    public class TheIsSingleActionBatchProperty
    {
        [TestCase]
        public void ReturnsFalseForEmptyBatch()
        {
            var batch = new Batch();

            Assert.That(batch.IsSingleActionBatch, Is.False);
        }

        [TestCase]
        public void ReturnsTrueForBatchWithOneAction()
        {
            var model = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new PropertyChangeUndo(model, "Value", model.Value));

            Assert.That(batch.IsSingleActionBatch, Is.True);
        }

        [TestCase]
        public void ReturnsFalseForBatchWithMultipleActions()
        {
            var model1 = new Mocks.MockModel();
            var model2 = new Mocks.MockModel();

            var batch = new Batch();
            batch.AddAction(new PropertyChangeUndo(model1, "Value", model1.Value));
            batch.AddAction(new PropertyChangeUndo(model2, "Value", model2.Value));

            Assert.That(batch.IsSingleActionBatch, Is.False);
        }
    }
    #endregion
}
