﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph.Collections;
using System.Diagnostics;
using QuickGraph.Predicates;
using QuickGraph.Algorithms.Observers;
using Xunit;

namespace QuickGraph.Tests.Collections
{
    public class FibonacciHeapTests
    {
        /// <summary>
        /// Checks heap invariant
        /// </summary>
        private static void AssertInvariant<TPriority, TValue>(
            FibonacciHeap<TPriority, TValue> target
            )
        {
            Assert.True(target.Count >= 0);
        }

        private void InsertAndRemoveMinimum<TPriority, TValue>(
            FibonacciHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            var count = target.Count;
            foreach (var kv in kvs)
                target.Enqueue(kv.Key, kv.Value);

            TPriority minimum = default(TPriority);
            for (int i = 0; i < kvs.Length; ++i)
            {
                if (i == 0)
                    minimum = target.Dequeue().Key;
                else
                {
                    var m = target.Dequeue().Key;
                    Assert.True(target.PriorityComparison(minimum, m) <= 0);
                    minimum = m;
                }
                AssertInvariant(target);
            }

            Assert.Equal(0, target.Count);
        }

        private void InsertAndMinimum<TPriority, TValue>(
            FibonacciHeap<TPriority, TValue> target,
            KeyValuePair<TPriority, TValue>[] kvs)
        {
            Assert.True(kvs.Length > 0);

            var count = target.Count;
            TPriority minimum = default(TPriority);
            for (int i = 0; i < kvs.Length; ++i)
            {
                KeyValuePair<TPriority, TValue> kv = kvs[i];
                if (i == 0)
                    minimum = kv.Key;
                else
                    minimum = target.PriorityComparison(kv.Key, minimum) < 0 ? kv.Key : minimum;
                target.Enqueue(kv.Key, kv.Value);
                // check minimum
                var kvMin = target.Top.Priority;
                Assert.Equal(minimum, kvMin);
            }
            AssertInvariant(target);
        }

        private void CompareBinary<TPriority, TValue>(
            KeyValuePair<bool, TPriority>[] values)
        {
            var fib = new FibonacciHeap<TPriority, TValue>();
            var bin = new BinaryHeap<TPriority, TValue>();
            foreach (var value in values)
            {
                /* TODO FIXME
                if (value.Key)
                    Assert.AreBehaviorsEqual(
                        () => fib.Enqueue(value.Value, default(TValue)),
                        () => bin.Add(value.Value, default(TValue))
                        );
                else
                {
                    Assert.AreBehaviorsEqual(
                        () => fib.Dequeue().Key,
                        () => bin.RemoveMinimum().Key
                        );
                }
                */
            }
        }

        private void Operations<TPriority, TValue>(
            FibonacciHeap<TPriority, TValue> target,
            KeyValuePair<bool, TPriority>[] values)
        {
            foreach (var value in values)
            {
                if (value.Key)
                    target.Enqueue(value.Value, default(TValue));
                else
                {
                    var min = target.Dequeue();
                }
                AssertInvariant<TPriority, TValue>(target);
            }
        }

        [Fact]
        public void CreateHeap()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            Assert.NotNull(heap);
        }

        [Fact]
        public void SimpleEnqueDequeIncreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }
            int? lastValue = null;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void SimpleEnqueDequeDecreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }
            int? lastValue = null;

            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue < value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void DecreaseKeyOnIncreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[9], -1);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void IncreaseKeyOnIncreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[0], 100);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void DecreaseKeyOnDecreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[9], -1);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue < value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void IncreaseKeyOnDecreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[0], 100);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                if (lastValue < value.Key)
                {
                    // Assert.Fail("Heap condition has been violated");
                    //FIXME TODO
                }
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void ChangeKeyToSelf()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Decreasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.ChangeKey(cells[0], 0);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue < value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void IncreasingDecreaseKeyCascadeCut()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            lastValue = heap.Top.Priority;
            heap.Dequeue();
            count--;
            heap.ChangeKey(cells[6], 3);
            heap.ChangeKey(cells[7], 2);
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void IncreasingIncreaseKeyCascadeCut()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            lastValue = heap.Top.Priority;
            heap.Dequeue();
            count--;
            heap.ChangeKey(cells[5], 10);
            string s = (heap as FibonacciHeap<int, string>).DrawHeap();
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void DeleteKey()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            var cells = new Dictionary<int, FibonacciHeapCell<int, string>>();
            for (int i = 0; i < 10; i++)
            {
                cells.Add(i, heap.Enqueue(i, i.ToString()));
                count++;
            }
            int? lastValue = null;
            heap.Dequeue();
            var DeletedCell = cells[8];
            heap.Delete(DeletedCell);
            count -= 2;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                //TODO FIXME Assert.NotEqual(DeletedCell, value);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void EnumeratorIncreasing()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            for (int i = 10; i >= 0; i--)
            {
                heap.Enqueue(i, i.ToString());
                count++;
            }
            int? lastValue = null;
            foreach (var value in heap)
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void MergeTest()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            FibonacciHeap<int, string> heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            int count = 0;
            for (int i = 11; i > 0; i--)
            {
                heap.Enqueue(i, i.ToString());
                heap2.Enqueue(i * 11, i.ToString());
                count += 2;
            }
            heap2.Merge(heap);
            int? lastValue = null;
            foreach (var value in heap2.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void NextCutOnLessThan()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var toCutNodes = new List<FibonacciHeapCell<int, string>>();
            int count = 0;
            heap.Enqueue(0, "0");
            toCutNodes.Add(heap.Enqueue(5, "5"));
            toCutNodes.Add(heap.Enqueue(6, "6"));
            toCutNodes.Add(heap.Enqueue(7, "7"));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            heap.Enqueue(1, "1");
            heap2.Enqueue(4, "4");
            heap2.Enqueue(5, "5");
            heap2.Enqueue(-10, "-10");
            heap2.Dequeue();
            heap.Merge(heap2);
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            toCutNodes.ForEach(x => heap.ChangeKey(x, -5));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            count = 7;
            int? lastValue = null;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void NextCutOnGreaterThan()
        {
            var heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var heap2 = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            var toCutNodes = new List<FibonacciHeapCell<int, string>>();
            int count = 0;
            heap.Enqueue(1, "1");
            toCutNodes.Add(heap.Enqueue(5, "5"));
            toCutNodes.Add(heap.Enqueue(6, "6"));
            toCutNodes.Add(heap.Enqueue(7, "7"));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            heap.Enqueue(0, "0");
            heap2.Enqueue(-1, "-1");
            heap2.Enqueue(5, "5");
            heap2.Enqueue(-10, "-10");
            heap2.Dequeue();
            heap.Merge(heap2);
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            toCutNodes.ForEach(x => heap.ChangeKey(x, -5));
            heap.Enqueue(-10, "-10");
            heap.Dequeue();
            count = 7;
            int? lastValue = null;
            foreach (var value in heap.GetDestructiveEnumerator())
            {
                if (lastValue == null)
                {
                    lastValue = value.Key;
                }
                Assert.False(lastValue > value.Key);
                lastValue = value.Key;
                count--;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void RandomTest()
        {
            FibonacciHeap<int, string> heap = new FibonacciHeap<int, string>(HeapDirection.Increasing);
            Random rand = new Random(10);
            int NumberOfRecords = 10000;
            int RangeMultiplier = 10;
            int count = 0;
            var cells = new List<FibonacciHeapCell<int, string>>();
            for (int i = 0; i < NumberOfRecords; i++)
            {
                cells.Add(heap.Enqueue(rand.Next(0, NumberOfRecords * RangeMultiplier), i.ToString()));
                count++;
            }
            while (!heap.IsEmpty)
            {
                int action = rand.Next(1, 6);
                int i = 0;
                while (action == 1 && i < 2)
                {
                    action = rand.Next(1, 6);
                    i++;
                }
                int lastValue = int.MinValue;
                switch (action)
                {
                    case 1:
                        cells.Add(heap.Enqueue(rand.Next(0, NumberOfRecords * RangeMultiplier), "SomeValue"));
                        count++;
                        break;
                    case 2:
                        Assert.False(lastValue > heap.Top.Priority);
                        lastValue = heap.Top.Priority;
                        cells.Remove(heap.Top);
                        heap.Dequeue();
                        count--;
                        break;
                    case 3:
                        int deleteIndex = rand.Next(0, cells.Count);
                        heap.Delete(cells[deleteIndex]);
                        cells.RemoveAt(deleteIndex);
                        count--;
                        break;
                    case 4:
                        int decreaseIndex = rand.Next(0, cells.Count);
                        int newValue = rand.Next(0, cells[decreaseIndex].Priority);
                        if (newValue < lastValue)
                        {
                            lastValue = newValue;
                        }
                        heap.ChangeKey(cells[decreaseIndex], newValue);
                        break;
                    case 5:
                        int increaseIndex = rand.Next(0, cells.Count);
                        heap.ChangeKey(cells[increaseIndex], rand.Next(cells[increaseIndex].Priority, NumberOfRecords * RangeMultiplier));
                        break;
                    default:
                        break;
                }
            }
            Assert.Equal(0, count);
        }
    }
}
