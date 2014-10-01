using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;

namespace VHA.ServiceFoundation
{
    [Serializable]
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class Criteria
    {
        [DataMember]
        public string FilterFieldName { get; set; }
        [DataMember]
        public object FilterFieldValue { get; set; }
        [DataMember]
        public string SortFieldName { get; set; }
        [DataMember]
        public SortDirection SortDirection { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public List<ComplexFilter> ComplexFilters { get; set; }
        [DataMember]
        public Dictionary<string, string> AdditionalInfo { get; set; }

        public Criteria()
        {
            this.SortDirection = SortDirection.Ascending;
            this.PageSize = 10;
            this.PageNumber = 1;
            this.ComplexFilters = new List<ComplexFilter>();
            this.AdditionalInfo = new Dictionary<string, string>();
        }

        public void ResetFilters()
        {
            this.FilterFieldName = null;
            this.FilterFieldValue = null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\nComplexFilters: [");
            this.ComplexFilters.ForEach(s => sb.AppendFormat("{0}", s.ToString()));
            sb.Append("]");
            sb.AppendFormat("\nFilterFieldName: {0}", this.FilterFieldName);
            sb.AppendFormat("\nFilterFieldValue: {0}", this.FilterFieldValue);
            sb.AppendFormat("\nSortFieldName: {0}", this.SortFieldName);
            sb.AppendFormat("\nSortDirection: {0}", this.SortDirection);
            sb.AppendFormat("\nPageNumber: {0}", this.PageNumber);
            sb.AppendFormat("\nPageSize: {0}", this.PageSize);

            return sb.ToString();
        }

        public static Criteria FromQueryString(NameValueCollection queryString, string defaultSortFieldName)
        {
            return new Criteria
            {
                ComplexFilters = ComplexFilter.FromJson(queryString["complexFilters"].EmptyToDefault(null)),
                FilterFieldName = queryString["filterFieldName"].EmptyToDefault(null),
                FilterFieldValue = queryString["filterFieldValue"].EmptyToDefault(null),
                PageNumber = queryString["pageNumber"].IsNumeric() ? Int32.Parse(queryString["pageNumber"]) : 1,
                PageSize = (queryString["pageSize"].IsNumeric()) ? Int32.Parse(queryString["pageSize"]) : 10,
                SortFieldName = queryString["sortFieldName"].EmptyToDefault(defaultSortFieldName),
                SortDirection = (new string[] { "DESC", "desc", "Desc" }.Contains(queryString["sortDirection"]))
                                    ? SortDirection.Descending
                                    : SortDirection.Ascending
            };
        }

    }
}
