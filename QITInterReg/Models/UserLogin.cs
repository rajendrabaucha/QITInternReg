﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QITInterReg.Models
{
    public class UserLogin
    {
        [Display(Name ="Email ID")] 
        [Required(AllowEmptyStrings =false,ErrorMessage ="Email Id is required")]
        public string EmailID { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Password is required")]
        public string  Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}