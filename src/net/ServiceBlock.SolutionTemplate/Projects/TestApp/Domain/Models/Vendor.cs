using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    [DataContract(Namespace = "Domain.Models")]
    [DisplayColumn("Name")]
    public class Vendor
    {
        [DataMember]
        [Key]
        public int VendorId { get; set; }

        [DataMember]
        public string Name { get; set; }

        private ICollection<Product> Products { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
