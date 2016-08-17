using Auto_Barter.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Auto_Barter.Models.CarModels;
using System.Data.Entity;

namespace Auto_Barter.Controllers
{
    public class CarController : Controller
    {
        JavaScriptSerializer serialiser = new JavaScriptSerializer();
        string apiKey = "968v7jffe8uw4teunhppq5r5";

        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new OurDbContext())
            {

                return View(db.CarPost.ToList());
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            string apiKey = "968v7jffe8uw4teunhppq5r5";
            var client = new RestClient("http://api.edmunds.com/api/vehicle/v2/");
            var request = new RestRequest($"makes?&view=basic&fmt=json&api_key={apiKey}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<RootObject>(response.Content);

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = string.Empty, Value = null });

            foreach (var item in content.makes)
            {
                items.Add(new SelectListItem
                {
                    Text = item.name,
                    Value = item.niceName
                });
            }

            ViewBag.Makes = items;
            return View();
        }

        [HttpPost]
        public ActionResult Create(CarPost post)
        {
            using (var db = new OurDbContext())
            {
                post.UserDetails = db.UserDetails.Include(x => x.Address)
                    .Include(x => x.UserAccount)
                    .FirstOrDefault(x => x.UserAccount.UserId == int.Parse(Session["UserId"].ToString()));

                post.SponsoredPost = false;
                post.PostDate = DateTime.UtcNow;
                db.SaveChanges();
            }
            return View();
        }


        // WEB METHODS
        [HttpPost]
        public string GetCarInfo(string vin)
        {
            var client = new RestClient("http://api.edmunds.com/v1/api/toolsrepository/");
            var request = new RestRequest($"vindecoder?vin={vin}&fmt=json&api_key={apiKey}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<root>(response.Content).styleHolder.FirstOrDefault();

            var Car = new Car
            {
                Make = content.makeName,
                Model = content.modelName,
                Year = content.year,
                Vin = vin
            };

            return serialiser.Serialize(Car);
        }

        [HttpPost]
        public string GetCarMakes(string year)
        {
            var client = new RestClient("http://api.edmunds.com/api/vehicle/v2/");
            var request = new RestRequest($"makes?&view=basic&fmt=json&year={year}&api_key={apiKey}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<RootObject>(response.Content);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in content.makes)
            {
                items.Add(new SelectListItem
                {
                    Text = item.name,
                    Value = item.niceName
                });
            }

            return serialiser.Serialize(items);
        }

        [HttpPost]
        public string GetCarModels(string make)
        {
            List<string> ModelNames = new List<string>();
            if (!string.IsNullOrEmpty(make))
            {
                var serialiser = new JavaScriptSerializer();

                if (make == "Choose a make")
                {
                    ModelNames.Clear();
                }
                else
                {
                    string apiKey = "968v7jffe8uw4teunhppq5r5";
                    var client = new RestClient("http://api.edmunds.com/api/vehicle/v2/");
                    var request = new RestRequest($"{make}/models?fmt=json&api_key={apiKey}");
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                    var response = client.Execute(request);
                    var content = JsonConvert.DeserializeObject<Make>(response.Content);
                    foreach (var item in content.models)
                    {
                        ModelNames.Add(item.name);

                    }
                }
            }
            return serialiser.Serialize(ModelNames);
        }

        public class root
        {
            public List<StyleHolder> styleHolder { get; set; }
        }

        public class StyleHolder
        {
            [DeserializeAs(Name = "makeId")]
            public int makeId { get; set; }

            [DeserializeAs(Name = "year")]
            public int year { get; set; }
            [DeserializeAs(Name = "makeName")]
            public string makeName { get; set; }

            [DeserializeAs(Name = "makeNiceName")]
            public string makeNiceName { get; set; }

            [DeserializeAs(Name = "modelId")]
            public string modelId { get; set; }

            [DeserializeAs(Name = "modelName")]
            public string modelName { get; set; }

            [DeserializeAs(Name = "modelNiceName")]
            public string modelNiceName { get; set; }

            [DeserializeAs(Name = "transmissionType")]
            public string transmissionType { get; set; }
        }
    }
}