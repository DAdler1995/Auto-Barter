using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Auto_Barter.Models.CarModels;

namespace Auto_Barter.Models
{

    public class CarPost
    {
        [Key]
        public int PostId { get; set; }
        
        [Required(ErrorMessage = "Post details are required.")]
        [AllowHtml, DataType(DataType.MultilineText)]
        public string Details { get; set; }

        public Car UserCar { get; set; }

        public DateTime PostDate { get; set; }
        public UserDetails UserDetails { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Desired Trades"), Required(ErrorMessage = "Please choose at least one car you're looking to trade for.")]
        public IEnumerable<Car> DesiredTrades { get; set; }

        [Display(Name = "Open to all offers?"), Required()]
        public bool OpenToOffers { get; set; }
        
        public bool SponsoredPost { get; set; }

        [Display(Name = "Additional cash offer")]
        public int AdditionalCash { get; set; }
    }
}