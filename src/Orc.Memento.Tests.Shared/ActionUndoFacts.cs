// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionUndoFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento.Tests
{
    using System;
    using Catel.Test;
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
                Assert.IsFalse(value);

                mementoService.Undo();
                Assert.IsTrue(value);

                mementoService.Redo();
                Assert.IsFalse(value);
            }

            [TestCase]
            public void SetProperty()
            {
                var instance = new MockCatelModel();
                var action = new PropertyChangeUndo(instance, "Value", "previousValue", "nextValue");
                var mementoService = new MementoService();

                mementoService.Add(action);
                Assert.AreEqual(MockCatelModel.ValueProperty.GetDefaultValue(), instance.Value);

                mementoService.Undo();
                Assert.AreEqual("previousValue", instance.Value);

                mementoService.Redo();
                Assert.AreEqual("nextValue", instance.Value);
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
                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => new ActionUndo(null, () => MockModel.Change("test")));
            }

            [TestCase]
            public void ThrowsArgumentExceptionForNullUndoMethod()
            {
                var obj = new object();

                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => new ActionUndo(obj, null));
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
                Assert.AreEqual("previousValue", MockModel.Name);
            }

            [TestCase]
            public void SetsNewValue()
            {
                var action = new ActionUndo(this, () => MockModel.Change("previousValue"), () => MockModel.Change("nextValue"));

                action.Redo();
                Assert.AreEqual("nextValue", MockModel.Name);
            }

            [TestCase]
            public void SetsOldAndNewValue()
            {
                var action = new ActionUndo(this, () => MockModel.Change("previousValue"), () => MockModel.Change("nextValue"));

                action.Undo();
                Assert.AreEqual("previousValue", MockModel.Name);

                action.Redo();
                Assert.AreEqual("nextValue", MockModel.Name);

                action.Undo();
                Assert.AreEqual("previousValue", MockModel.Name);
            }
        }
        #endregion
    }
}