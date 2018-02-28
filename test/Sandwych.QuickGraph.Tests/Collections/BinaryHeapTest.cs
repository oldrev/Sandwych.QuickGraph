using System;
using System.Collections.Generic;
using System.Collections;
using Xunit;

namespace QuickGraph.Collections
{
    public static class BinaryHeapFactory
    {
        public static BinaryHeap<int, int> Create(int capacity)
        {
            var heap = new BinaryHeap<int, int>(capacity, (i, j) => i.CompareTo(j));
            return heap;
        }
    }

    /// <summary>
    /// This class contains parameterized unit tests for BinaryHeap`2
    /// </summary>
    public class BinaryHeapTPriorityTValueTest
    {
        /// <summary>
        /// Checks heap invariant
        /// </summary>
        private static void AssertInvariant<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target
            )
        {
            Assert.True(target.Capacity >= 0);
            Assert.True(target.Count >= 0);
            Assert.True(target.Count <= target.Capacity);
        }

        [Fact]
        public void DefaultConstructorTest()
        {
            var target = new BinaryHeap<int, int>();
            AssertInvariant<int, int>(target);
        }

        [Theory]
        [InlineData(1), InlineData(3), InlineData(9), InlineData(8)]
        public void ConstructorTest(int capacity)
        {
            var target = new BinaryHeap<int, int>(capacity, Comparer<int>.Default.Compare);
            Assert.Equal(target.Capacity, capacity);
            AssertInvariant<int, int>(target);
        }

        private void Operations<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<bool, TPriority>[] values)
        {
            foreach (var value in values)
            {
                if (value.Key)
                    target.Add(value.Value, default(TValue));
                else
                {
                    var min = target.RemoveMinimum();
                }
                AssertInvariant<TPriority, TValue>(target);
            }
        }

        private void Insert<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            var count = target.Count;
            foreach (var kv in kvs)
            {
                target.Add(kv.Key, kv.Value);
                AssertInvariant<TPriority, TValue>(target);
            }
            Assert.True(count + kvs.Length == target.Count);
        }

        private void InsertAndIndexOf<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            foreach (var kv in kvs)
                target.Add(kv.Key, kv.Value);
            foreach (var kv in kvs)
                Assert.True(target.IndexOf(kv.Value) > -1);
        }

        private void InsertAndRemoveAt<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs,
            int removeAtIndex)
        {
            foreach (var kv in kvs)
                target.Add(kv.Key, kv.Value);
            var count = target.Count;
            var removed = target.RemoveAt(removeAtIndex);
            Assert.Equal(count - 1, target.Count);
            AssertInvariant<TPriority, TValue>(target);
        }

        private void InsertAndEnumerate<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            var dic = new Dictionary<TPriority, TValue>();
            foreach (var kv in kvs)
            {
                target.Add(kv.Key, kv.Value);
                dic[kv.Key] = kv.Value;
            }
            Assert.All(target, kv => dic.ContainsKey(kv.Key));
        }

        private void InsertAndRemoveMinimum<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            var count = target.Count;
            foreach (var kv in kvs)
                target.Add(kv.Key, kv.Value);

            TPriority minimum = default(TPriority);
            for (int i = 0; i < kvs.Length; ++i)
            {
                if (i == 0)
                    minimum = target.RemoveMinimum().Key;
                else
                {
                    var m = target.RemoveMinimum().Key;
                    Assert.True(target.PriorityComparison(minimum, m) <= 0);
                    minimum = m;
                }
                AssertInvariant(target);
            }

            Assert.Equal(0, target.Count);
        }

        [Fact]
        public void RemoveMinimumOnEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new BinaryHeap<int, int>().RemoveMinimum();
            });
        }

        private void InsertAndMinimum<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            Assert.True(kvs.Length > 0);

            var count = target.Count;
            TPriority minimum = default(TPriority);
            for (int i = 0; i < kvs.Length; ++i)
            {
                var kv = kvs[i];
                if (i == 0)
                    minimum = kv.Key;
                else
                    minimum = target.PriorityComparison(kv.Key, minimum) < 0 ? kv.Key : minimum;
                target.Add(kv.Key, kv.Value);
                // check minimum
                var kvMin = target.Minimum();
                Assert.Equal(minimum, kvMin.Key);
            }
            AssertInvariant<TPriority, TValue>(target);
        }

        [Fact]
        public void MinimumOnEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new BinaryHeap<int, int>().Minimum();
            });
        }
    }


    public class BinaryHeapTPriorityTValueEnumeratorTest
    {
        private void InsertManyAndEnumerateUntyped<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            foreach (var kv in kvs)
                target.Add(kv.Key, kv.Value);
            foreach (KeyValuePair<TPriority, TValue> kv in (IEnumerable)target) ;
        }

        private void InsertManyAndDoubleForEach<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            foreach (var kv in kvs)
                target.Add(kv.Key, kv.Value);
            //TODO FIXME PexEnumerablePatterns.DoubleForEach(target);
        }

        private void InsertManyAndMoveNextAndReset<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            foreach (var kv in kvs)
                target.Add(kv.Key, kv.Value);
            //TODO FIXME PexEnumerablePatterns.MoveNextAndReset(target);
        }

        private void InsertAndMoveNextAndModify<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue> kv)
        {
            target.Add(kv.Key, kv.Value);
            Assert.Throws<InvalidOperationException>(delegate
            {
                var enumerator = target.GetEnumerator();
                target.Add(kv.Key, kv.Value);
                enumerator.MoveNext();
            });
        }

        private void InsertAndResetAndModify<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue> kv)
        {
            target.Add(kv.Key, kv.Value);
            Assert.Throws<InvalidOperationException>(delegate
            {
                var enumerator = target.GetEnumerator();
                target.Add(kv.Key, kv.Value);
                enumerator.Reset();
            });
        }

        private void InsertAndCurrentAndModify<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue> kv)
        {
            target.Add(kv.Key, kv.Value);
            Assert.Throws<InvalidOperationException>(delegate
            {
                var enumerator = target.GetEnumerator();
                target.Add(kv.Key, kv.Value);
                var current = enumerator.Current;
            });
        }

        private void CurrentAfterMoveNextFinished<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue> kv)
        {
            target.Add(kv.Key, kv.Value);
            Assert.Throws<InvalidOperationException>(delegate
            {
                var enumerator = target.GetEnumerator();
                while (enumerator.MoveNext()) ;
                var current = enumerator.Current;
            });
        }

        private void CurrentBeforeMoveNext<TPriority, TValue>(
            BinaryHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue> kv)
        {
            target.Add(kv.Key, kv.Value);
            Assert.Throws<InvalidOperationException>(delegate
            {
                var enumerator = target.GetEnumerator();
                var current = enumerator.Current;
            });
        }
    }
}
