using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auto_Barter.Models
{

    public enum TitleStatus
    {
        CLEAN,
        SALVAGE,
        REBUILT,
        LEIN
    }
    public enum Drivetype
    {
        FWD,
        RWD,
        AWD
    }

    public enum Transmission
    {
        MANUAL,
        AUTOMATIC,
        AUTOMANUAL,
        CVT,
        OTHER
    }

    public class CarModels
    {
        public class Year
        {
            public int id { get; set; }
            public int year { get; set; }
        }

        public class Model
        {
            public string id { get; set; }
            public string name { get; set; }
            public string niceName { get; set; }
            public List<Year> years { get; set; }
        }

        public class Make
        {
            [DeserializeAs(Name = "id")]
            public int id { get; set; }
            [DeserializeAs(Name = "name")]
            public string name { get; set; }
            [DeserializeAs(Name = "niceName")]
            public string niceName { get; set; }
            [DeserializeAs(Name = "models")]
            public List<Model> models { get; set; }
        }

        public class RootObject
        {
            [DeserializeAs(Name = "makes")]
            public IEnumerable<Make> makes { get; set; }
            [DeserializeAs(Name = "makeCount")]
            public int makesCount { get; set; }
        }


        public class Car
        {
            [Key]
            public int CarId { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public string Vin { get; set; }

            [Required(ErrorMessage = "Car mileage is required.")]
            public int Mileage { get; set; }

            [Display(Name = "Car Title Status"), Required(ErrorMessage = "Title is required.")]
            public TitleStatus Title { get; set; }

            [Required(ErrorMessage = "Transmission type is required")]
            public Transmission Transmission { get; set; }

            [Required(ErrorMessage = "Drivetype type is required")]
            public Drivetype Drivetype { get; set; }

            [Display(Name = "Exterior Color"), Required(ErrorMessage = "Exterior color is required.")]
            public string ExteriorColor { get; set; }

            [Display(Name = "Interior Color")]
            public string InteriorColor { get; set; }

            public DateTime EnteredDateTime { get; set; }
        }
    }
}