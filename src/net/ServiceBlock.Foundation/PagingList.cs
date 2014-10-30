using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ServiceBlock.Foundation
{
    [ExcludeFromCodeCoverage]
    public class PagingList<T> : List<T>
    {
        public PagingList()
            : base()
        {
        }

        public PagingList(int capacity)
            : base(capacity)
        {
        }

        public PagingList(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public int TotalCount { get; set; }
    }
}
