using System;
using System.Collections.Generic;
using System.Text;

namespace QuickGraph.Tests.Factories
{
    public sealed class StringVertexFactory
    {
        private int id = 0;

        public StringVertexFactory()
            : this("Super")
        { }

        public StringVertexFactory(string prefix)
        {
            this.Prefix = prefix;
        }

        public string Prefix { get; set; }

        public string CreateVertex()
        {
            return this.Prefix + (++id).ToString();
        }
    }
}
