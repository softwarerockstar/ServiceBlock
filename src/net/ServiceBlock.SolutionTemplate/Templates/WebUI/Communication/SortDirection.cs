﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace $safeprojectname$.Communication
{
    public enum SortDirection
    {
        [EnumMember(Value = "Asc")]
        Ascending,

        [EnumMember(Value = "Desc")]
        Descending
    }
}