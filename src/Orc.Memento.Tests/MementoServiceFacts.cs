namespace Orc.Memento.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using Mocks;
using NUnit.Framework;

public class MementoServiceFacts
{
    #region Nested type: TheBeginBatchMethod
    [TestFixture]
    public class TheBeginBatchMethod
    {
        [TestCase]
        public void BeginsNewBatchWhenThereAlreadyIsABatch()
        {
            var mementoService = new MementoService();
            var model = new MockModel();

            var firstBatch = mementoService.BeginBatch("FirstBatch");
            mementoService.Add(new PropertyChangeUndo(model, "Value", model.Value));
            Assert.That(firstBatch.ActionCount, Is.EqualTo(1));

            var secondBatch = mementoService.BeginBatch("SecondBatch");
            mementoService.Add(new PropertyChangeUndo(model, "Value", model.Value));
            Assert.That(secondBatch.ActionCount, Is.EqualTo(1));

            // Also check if the first batch was closed
            Assert.That(mementoService.UndoBatches.Count(), Is.EqualTo(1));
            Assert.That(firstBatch.ActionCount, Is.EqualTo(1));
        }

        [TestCase]
        public void UndoBatches()
        {
            var model = new MockModel();
            var mementoService = new MementoService();
            mementoService.RegisterObject(model);

            string originalNumber = model.Value;

            mementoService.BeginBatch();

            model.Value = "1000";
            model.Value = "100";
            model.Value = "10";

            var mementoBatch = mementoService.EndBatch();

            mementoBatch.Undo();

            Assert.That(model.Value, Is.EqualTo(originalNumber));
        }
    }
    #endregion

    #region Nested type: TheConstructor
    [TestFixture]
    public class TheConstructor
    {
        #region Methods
        [TestCase]
        public void ThrowsArgumentOutOfRangeExceptionForNegativeParameter()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MementoService(-1));
        }

        [TestCase]
        public void ExpectDefaultMaximumSupportedActionsValue()
        {
            var mementoService = new MementoService();
            Assert.That(mementoService.MaximumSupportedBatches, Is.EqualTo(300));
        }
        #endregion
    }
    #endregion

    #region Nested type: TheEndBatchMethod
    [TestFixture]
    public class TheEndBatchMethod
    {
        [TestCase]
        public void EndsBatchWhenThereAlreadyIsABatch()
        {
            var mementoService = new MementoService();
            var model = new MockModel();

            var firstBatch = mementoService.BeginBatch("FirstBatch");
            mementoService.Add(new PropertyChangeUndo(model, "Value", model.Value));
            Assert.That(firstBatch.ActionCount, Is.EqualTo(1));

            var secondBatch = mementoService.BeginBatch("SecondBatch");
            mementoService.Add(new PropertyChangeUndo(model, "Value", model.Value));
            Assert.That(secondBatch.ActionCount, Is.EqualTo(1));
            mementoService.EndBatch();

            Assert.That(mementoService.UndoBatches.Count(), Is.EqualTo(2));
        }
    }
    #endregion

    #region Nested type: TheIsEnabledProperty
    [TestFixture]
    public class TheIsEnabledProperty
    {
        [TestCase]
        public void IsTrueByDefault()
        {
            var mementoService = new MementoService();

            Assert.That(mementoService.IsEnabled, Is.True);
        }

        [TestCase]
        public void PreventsAdditionsWhenDisabled()
        {
            var mementoService = new MementoService();
            mementoService.IsEnabled = false;

            var undo1 = new MockUndo(true);
            mementoService.Add(undo1);

            Assert.That(mementoService.CanRedo, Is.False);
        }
    }
    #endregion

    #region Nested type: TheMaximumSupportedProperty
    [TestFixture]
    public class TheMaximumSupportedProperty
    {
        #region Methods
        [TestCase]
        public void MaximumSupportedOperationsTest()
        {
            var mementoService = new MementoService(5);
            var listUndoOps = new List<MockUndo>();

            for (var i = 0; i < 10; i++)
            {
                var memento = new MockUndo() {Value = i};
                mementoService.Add(memento);
                listUndoOps.Add(memento);
            }

            var count = 0;
            while (mementoService.CanUndo)
            {
                mementoService.Undo();
                count++;
            }

            for (var i = 0; i < 5; i++)
            {
                Assert.That(listUndoOps[i].UndoCalled, Is.False);
            }

            for (var i = 5; i < 10; i++)
            {
                Assert.That(listUndoOps[i].UndoCalled, Is.True);
            }

            Assert.That(mementoService.MaximumSupportedBatches, Is.EqualTo(count));
        }
        #endregion
    }
    #endregion

    #region Nested type: TheRedoMethod
    [TestFixture]
    public class TheRedoMethod
    {
        #region Methods
        [TestCase]
        public void RedoTest()
        {
            var mementoService = new MementoService();
            var undo1 = new MockUndo(true);

            mementoService.Add(undo1);
            mementoService.Undo();
            Assert.That(undo1.UndoCalled, Is.True);
            Assert.That(undo1.RedoCalled, Is.False);
            Assert.That(mementoService.CanRedo, Is.True);

            mementoService.Redo();
            Assert.That(undo1.RedoCalled, Is.True);
            Assert.That(mementoService.CanRedo, Is.False);
        }

        [TestCase]
        public void HandlesDoubleRedo()
        {
            var obj = new MockModel {Value = "value1"};
            var service = new MementoService();

            service.RegisterObject(obj);
            obj.Value = "value2";
            obj.Value = "value3";

            service.Undo();
            Assert.That(obj.Value, Is.EqualTo("value2"));

            service.Undo();
            Assert.That(obj.Value, Is.EqualTo("value1"));

            service.Redo();
            Assert.That(obj.Value, Is.EqualTo("value2"));

            service.Redo();
            Assert.That(obj.Value, Is.EqualTo("value3"));
        }

        [TestCase]
        public void CanRedoTest()
        {
            var mementoService = new MementoService();
            Assert.That(mementoService.CanUndo, Is.False);

            mementoService.Add(new MockUndo());
            Assert.That(mementoService.CanUndo, Is.True);

            mementoService.Undo();
            Assert.That(mementoService.CanRedo, Is.False);

            mementoService.Add(new MockUndo(true));
            Assert.That(mementoService.CanUndo, Is.True);

            mementoService.Undo();
            Assert.That(mementoService.CanUndo, Is.False);
            Assert.That(mementoService.CanRedo, Is.True);

            mementoService.Redo();
            Assert.That(mementoService.CanUndo, Is.True);
            Assert.That(mementoService.CanRedo, Is.False);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public void RaisesUpdatedEvent(int actionsCount)
        {
            var mementoService = new MementoService();
            var raisedEventsCount = 0;
            mementoService.Updated += (sender, args) => 
            {
                if(args.MementoAction == MementoAction.Redo)
                {
                    raisedEventsCount++;
                }
            };

            for (var i = 0; i < actionsCount; i++)
            {
                mementoService.Add(new MockUndo(true));
            }

            for (var i = 0; i < actionsCount; i++)
            {
                mementoService.Undo();
            }

            for (var i = 0; i < actionsCount; i++)
            {
                mementoService.Redo();
            }

            Assert.That(raisedEventsCount, Is.EqualTo(actionsCount));
        }

        #endregion
    }
    #endregion

    #region Nested type: TheUndoMethod
    [TestFixture]
    public class TheUndoMethod
    {
        #region Methods
        [TestCase]
        public void UndoTest()
        {
            var mementoService = new MementoService();
            var undo1 = new MockUndo();
            var undo2 = new MockUndo();

            mementoService.Add(undo1);
            mementoService.Add(undo2);

            mementoService.Undo();
            Assert.That(undo2.UndoCalled, Is.True);
            Assert.That(undo1.UndoCalled, Is.False);
            Assert.That(mementoService.CanUndo, Is.True);
        }

        [TestCase]
        public void HandlesDoubleUndo()
        {
            var obj = new MockModel {Value = "value1"};
            var service = new MementoService();

            service.RegisterObject(obj);

            obj.Value = "value2";
            obj.Value = "value3";

            service.Undo();
            Assert.That(obj.Value, Is.EqualTo("value2"));

            service.Undo();
            Assert.That(obj.Value, Is.EqualTo("value1"));
        }

        [TestCase]
        public void CanUndoTest()
        {
            var mementoService = new MementoService();
            Assert.That(mementoService.CanUndo, Is.False);

            mementoService.Add(new MockUndo());
            Assert.That(mementoService.CanUndo, Is.True);

            mementoService.Undo();
            Assert.That(mementoService.CanUndo, Is.False);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public void RaisesUpdatedEvent(int actionsCount)
        {
            var mementoService = new MementoService();
            var raisedEventsCount = 0;
            mementoService.Updated += (sender, args) =>
            {
                if (args.MementoAction == MementoAction.Undo)
                {
                    raisedEventsCount++;
                }
            };

            for (var i = 0; i < actionsCount; i++)
            {
                mementoService.Add(new MockUndo(true));
            }

            for (var i = 0; i < actionsCount; i++)
            {
                mementoService.Undo();
            }

            Assert.That(raisedEventsCount, Is.EqualTo(actionsCount));
        }

        #endregion
    }
    #endregion

    #region Nested type: TheUnregisterObjectMethod
    [TestFixture]
    public class TheUnregisterObjectMethod
    {
        #region Methods
        [TestCase]
        public void ThrowsArgumentNullExceptionForNullInstance()
        {
            var service = new MementoService();

            Assert.Throws<ArgumentNullException>(() => service.UnregisterObject(null));
        }

        [TestCase]
        public void CancelsSubscriptionForInstance()
        {
            var obj = new MockModel {Value = "value1"};
            var service = new MementoService();

            service.RegisterObject(obj);
            service.UnregisterObject(obj);

            obj.Value = "newvalue";

            Assert.That(service.CanUndo, Is.False);
        }

        [TestCase]
        public void ClearsCurrentUndoRedoStackForInstance()
        {
            var obj = new MockModel {Value = "value1"};
            var service = new MementoService();

            service.RegisterObject(obj);

            obj.Value = "newvalue1";
            Assert.That(service.CanRedo, Is.False);

            service.UnregisterObject(obj);

            Assert.That(service.CanUndo, Is.False);
        }
        #endregion
    }
    #endregion

    [TestFixture]
    public class TheChangeNotifications
    {
        [TestCase]
        public void RaisesChangeRecordedEvent()
        {
            var mementoService = new MementoService();

            var success = false;

            mementoService.Updated += (sender, e) =>
            {
                if (e.MementoAction == MementoAction.ChangeRecorded)
                {
                    success = true;
                }
            };

            var undo1 = new MockUndo();

            mementoService.Add(undo1);

            Assert.That(success, Is.True);
        }

        [TestCase]
        public void RaisesClearDataEvent()
        {
            var mementoService = new MementoService();

            var success = false;

            mementoService.Updated += (sender, e) =>
            {
                if (e.MementoAction == MementoAction.ClearData)
                {
                    success = true;
                }
            };

            mementoService.Clear();

            Assert.That(success, Is.True);
        }
    }
}
