// <copyright file="SoftHeapTKeyTValueTest.cs" company="MSIT">Copyright © MSIT 2008</copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace QuickGraph.Collections
{
    /// <summary>This class contains parameterized unit tests for SoftHeap`2</summary>
    public class SoftHeapTKeyTValueTest
    {

        public static IEnumerable<object[]> KeysData => new List<object[]> {
                new object[] { new int[] { 1, 2, 3 } },
                new object[] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 } },
            };

        //TODO FIXME
        //[Theory]
        //[MemberData(nameof(KeysData))]
        public void Add(IEnumerable<int> keys)
        {
            Assert.True(keys.All(k => k < int.MaxValue));
            Assert.True(keys.Count() > 0);

            var target = new SoftHeap<int, int>(1 / 4.0, int.MaxValue);
            TestConsole.WriteLine("expected error rate: {0}", target.ErrorRate);
            foreach (var key in keys)
            {
                var count = target.Count;
                target.Add(key, key + 1);
                Assert.Equal(count + 1, target.Count);
            }

            int lastMin = int.MaxValue;
            int error = 0;
            while (target.Count > 0)
            {
                var kv = target.DeleteMin();
                if (lastMin < kv.Key)
                    error++;
                lastMin = kv.Key;
                Assert.Equal(kv.Key + 1, kv.Value);
            }

            TestConsole.WriteLine("error rate: {0}", error / (double)keys.Count());
            Assert.True(error / (double)keys.Count() <= target.ErrorRate);
        }
    }
}
