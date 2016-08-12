using Auto_Barter.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Auto_Barter.Models
{
    public class UserDetails
    {
        [Key]
        public int UserDetailsId { get; set; }

        public UserAccount UserAccount { get; set; }

        public Address Address { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}