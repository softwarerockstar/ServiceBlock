using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceBlock.Foundation
{
    [DataContract]
    public enum SortDirection
    {
        [EnumMember(Value="Asc")]
        Ascending,

        [EnumMember(Value = "Desc")]
        Descending
    }
}
