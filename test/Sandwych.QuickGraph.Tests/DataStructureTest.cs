using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace QuickGraph.Tests
{
    public class DataStructureTest
    {
        [Fact]
        public void DisplayLinkedList()
        {
            var target = new LinkedList<int>();
            target.AddFirst(0);
            target.AddFirst(1);
        }
    }
}
