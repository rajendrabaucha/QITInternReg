using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QITInterReg.Models
{
    [MetadataType(typeof(CustomerDto))]
    public partial class Customer
    {
    }


    public class CustomerDto
    {
        [Display(Name ="First Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="First name required.")]
        public string FName { get; set; }

        [Display(Name ="Last Name")]
        public string LName { get; set; }
    }

}