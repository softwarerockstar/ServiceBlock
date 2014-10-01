using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

/// These two classes demonstrate how model validations can be easily added using data annotations.
/// See http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx for a 
/// comprehensive list of annotations that can easily be added to properties of the model for easy validation.
/// Keep in mind that these validations occur on the web server.  If you want client-side validation
/// look at jQuery validation plugin at http://jqueryvalidation.org/

// Namespace must be the same as the service proxy namespace
namespace WebUI.Services.ProductSvc
{
    // A partial class with a special attribute that tells framework that 
    // annotations are provided in a buddy class named ProductAnnotations.
    [MetadataType(typeof(ProductAnnotations))]
    public partial class Product
    {
    }

    // Annotations class that duplicates properties as on the entity class with 
    // additional validation attributes.
    public class ProductAnnotations
    {
        [MaxLength(50, ErrorMessage="Max length of Name field is 50.")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Release Date is required.")]
        public DateTime ReleaseDate { get; set; }
    }
}