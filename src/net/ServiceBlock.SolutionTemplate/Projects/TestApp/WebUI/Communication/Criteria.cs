using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Communication
{
    [Serializable]
    public class Criteria
    {
        public string FilterFieldName { get; set; }
        public object FilterFieldValue { get; set; }
        public string SortFieldName { get; set; }
        public SortDirection SortDirection { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<ComplexFilter> ComplexFilters { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; }

        public Criteria()
        {
            this.SortDirection = SortDirection.Ascending;
            this.PageSize = 10;
            this.PageNumber = 1;
            this.ComplexFilters = new List<ComplexFilter>();
            this.AdditionalInfo = new Dictionary<string, string>();
        }
    }
}