namespace Orc.Memento.Tests;

using System;
using System.Collections.Generic;
using Catel.Collections;
using NUnit.Framework;

public class CollectionChangeUndoFacts
{
    [TestFixture]
    public class TheConstructor
    {
        [TestCase]
        public void ThrowsArgumentNullExceptionForNullInstance()
        {
            Assert.Throws<ArgumentNullException>(() => new CollectionChangeUndo(null, CollectionChangeType.Add, 0, 0, null, null));
        }

        [TestCase]
        public void SetsValuesCorrectly()
        {
            var table = new List<object>();
            var collectionChangeUndo = new CollectionChangeUndo(table, CollectionChangeType.Add, 0, 0, "currentValue", "nextValue");

            Assert.That(collectionChangeUndo.Collection, Is.Not.Null);
            Assert.That(collectionChangeUndo.ChangeType, Is.EqualTo(CollectionChangeType.Add));
            Assert.That(collectionChangeUndo.Collection, Is.EqualTo(table));
            Assert.That(collectionChangeUndo.OldValue, Is.EqualTo("currentValue"));
            Assert.That(collectionChangeUndo.NewValue, Is.EqualTo("nextValue"));
            Assert.That(collectionChangeUndo.CanRedo, Is.EqualTo(true));
        }
    }

    [TestFixture]
    public class TheRedoMethod
    {
        [TestCase]
        public void HandlesCollectionAddCorrectly()
        {
            var table = new List<string>();
            var tableAfter = new List<string>(new[] {"currentValue"});

            var collectionChangeUndo = new CollectionChangeUndo(table, CollectionChangeType.Add, 0, 1, null, "currentValue");
            collectionChangeUndo.Redo();

            Assert.That(CollectionHelper.IsEqualTo(table, tableAfter), Is.True);
        }

        // TODO: Write replace, remove, move
    }

    [TestFixture]
    public class TheUndoMethod
    {
        [TestCase]
        public void HandlesCollectionAddCorrectly()
        {
            var table = new List<string>(new[] {"currentValue"});
            var tableAfter = new List<string>();

            var collectionChangeUndo = new CollectionChangeUndo(table, CollectionChangeType.Add, 0, 1, null, "currentValue");
            collectionChangeUndo.Undo();

            Assert.That(CollectionHelper.IsEqualTo(table, tableAfter), Is.True);
        }

        // TODO: Write replace, remove, move
    }
}
