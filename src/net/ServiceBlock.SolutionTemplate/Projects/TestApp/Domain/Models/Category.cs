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
    public class Category
    {
        [DataMember]
        [Key]
        public int CategoryId { get; set; }

        [DataMember]
        [Required(ErrorMessage="Category Name is required.")]
        public string Name { get; set; }

        private ICollection<Product> Products { get; set; }                
    }
}
