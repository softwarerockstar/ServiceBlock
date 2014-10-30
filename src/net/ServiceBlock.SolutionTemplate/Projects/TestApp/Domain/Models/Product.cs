using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    //TODO: Optionally override ToString methods of all entities; it can be traced at message level anyway
    //TODO: Optionally Override ToString methods of all requests and responses; it can be traced at message level anyway
    [DataContract(Namespace = "Domain.Models")]
    [DisplayColumn("Name")]
    public class Product
    {
        [DataMember]
        [Key]
        public int ProductId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public int VendorId { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Product Name is required.")]
        [MaxLength(50, ErrorMessage="Product name length must not be greater than 50.")]
        [RegularExpression(@"^\d*[a-zA-Z ][a-zA-Z0-9 ]*$", ErrorMessage = "Invalid product name")]
        public string Name { get; set; }

        [DataMember]
        public DateTime ReleaseDate { get; set; }

        [DataMember]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [DataMember]
        [ForeignKey("VendorId")]        
        public virtual Vendor Vendor { get; set; }

    }
}
