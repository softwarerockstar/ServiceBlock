using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace ServiceBlock.Foundation
{
    // TODO: should use this class instead of PagingList

    [DataContract]
    [ExcludeFromCodeCoverage]
    public class PagedResponse<T> where T : class
    {
        [DataMember]
        public List<T> Body { get; set; }
        
        [DataMember]
        public int TotalCount { get; set; }

        public PagedResponse(PagingList<T> inner)
        {
            this.Body = new List<T>(inner);
            this.TotalCount = inner.TotalCount;
        }
    }
}
