using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto_Barter.Models
{
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

        public class makes
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
            public IEnumerable<makes> makes { get; set; }
            [DeserializeAs(Name = "makeCount")]
            public int makesCount { get; set; }
        }
    }
}