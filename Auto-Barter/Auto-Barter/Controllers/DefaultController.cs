using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Auto_Barter.Models.CarModels;

namespace Auto_Barter.Controllers
{
    public class DefaultController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            string apiKey = "968v7jffe8uw4teunhppq5r5";
            var client = new RestClient("http://api.edmunds.com/api/vehicle/v2/");
            var request = new RestRequest($"makes?&view=basic&fmt=json&api_key={apiKey}");
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

            ViewBag.Makes = items;

            return View();
        }

        [HttpPost]
        public string GetModel(string make)
        {
            List<string> ModelNames = new List<string>();
            var serialiser = new JavaScriptSerializer();

            string apiKey = "968v7jffe8uw4teunhppq5r5";
            var client = new RestClient("http://api.edmunds.com/api/vehicle/v2/");
            var request = new RestRequest($"{make}?&view=basic&fmt=json&api_key={apiKey}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<makes>(response.Content);
            foreach (var item in content.models)
            {
                ModelNames.Add(item.name);

            }

            var models = serialiser.Serialize(ModelNames);

            return models;
        }
    }
}