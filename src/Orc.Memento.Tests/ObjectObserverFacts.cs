namespace Orc.Memento.Tests;

using System;
using System.Linq;
using NUnit.Framework;

public class ObjectObserverFacts
{
    [TestFixture]
    public class TheBehavior
    {
        [TestCase]
        public void CorrectlyIgnoresDuplicatePropertyChangesWithEqualValues()
        {
            var obj = new Mocks.MockModel();

            var service = new MementoService();
            var observer = new ObjectObserver(obj, mementoService: service);

            Assert.That(service.UndoBatches.Count(), Is.EqualTo(0));

            obj.Value = "A";

            Assert.That(service.UndoBatches.Count(), Is.EqualTo(1));

            obj.Value = "A";

            Assert.That(service.UndoBatches.Count(), Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class TheConstructor
    {
        [TestCase]
        public void ThrowsArgumentNullExceptionForNullPropertyChanged()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectObserver(null, null, new MementoService()));
        }

        [TestCase]
        public void SetsValuesCorrectly()
        {
            var obj = new Mocks.MockModel();
            const string tag = "MyTag";

            var service = new MementoService();
            var observer = new ObjectObserver(obj, tag, service);

            Assert.That(observer.Tag, Is.EqualTo(tag));
        }
    }
}
