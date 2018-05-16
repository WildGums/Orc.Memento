// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectObserverFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento.Tests
{
    using System;
    using System.Linq;
    using Catel.Tests;
    using NUnit.Framework;

    public class ObjectObserverFacts
    {
        #region Nested type: TheBehavior
        [TestFixture]
        public class TheBehavior
        {
            [TestCase]
            public void CorrectlyIgnoresDuplicatePropertyChangesWithEqualValues()
            {
                var obj = new Mocks.MockModel();

                var service = new MementoService();
                var observer = new ObjectObserver(obj, mementoService: service);

                Assert.AreEqual(0, service.UndoBatches.Count());

                obj.Value = "A";

                Assert.AreEqual(1, service.UndoBatches.Count());

                obj.Value = "A";

                Assert.AreEqual(1, service.UndoBatches.Count());
            }
        }
        #endregion

        #region Nested type: TheConstructor
        [TestFixture]
        public class TheConstructor
        {
            [TestCase]
            public void ThrowsArgumentNullExceptionForNullPropertyChanged()
            {
                ExceptionTester.CallMethodAndExpectException<ArgumentNullException>(() => new ObjectObserver(null, null, new MementoService()));
            }

            [TestCase]
            public void SetsValuesCorrectly()
            {
                var obj = new Mocks.MockModel();
                var tag = "MyTag";

                var service = new MementoService();
                var observer = new ObjectObserver(obj, tag, service);

                Assert.AreEqual(tag, observer.Tag);
            }
        }
        #endregion
    }
}