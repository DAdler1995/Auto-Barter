using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auto_Barter.Models.Common
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Display(Name = "Address Line 1")]
        public string Street1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Street2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "ZIP/Postal Code")]
        public int PostalCode { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}