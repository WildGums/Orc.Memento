// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyChangeUndoFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento.Tests
{
    using System;
    using Catel.Test;
    using Mocks;
    using NUnit.Framework;

    public class PropertyChangeUndoFacts
    {
        #region Nested type: TheConstructor
        [TestFixture]
        public class TheConstructor
        {
            [TestCase]
            public void ThrowsArgumentNullExceptionForNullInstance()
            {
                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => new PropertyChangeUndo(null, "PropertyName", null));
            }

            [TestCase]
            public void ThrowsArgumentExceptionForNullPropertyName()
            {
                var obj = new object();

                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => new PropertyChangeUndo(obj, null, null));
                ExceptionTester.CallMethodAndExpectException<ArgumentException>(() => new PropertyChangeUndo(obj, string.Empty, null));
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
        #endregion

        #region Nested type: TheRedoMethod
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
        #endregion

        #region Nested type: TheUndoMethod
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
        #endregion
    }
}