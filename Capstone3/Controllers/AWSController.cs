using Capstone3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

namespace Capstone3.Controllers
{
    public class AWSController : Controller
    {
        public IActionResult Index()
        {
            
            Services serviceList = new Services();

            //gets all available AWS servise from API
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";


            var products = webClient.DownloadString("https://pricing.us-east-1.amazonaws.com/offers/v1.0/aws/index.json");
            JObject servicesQuryable = JObject.Parse(products);

            //populates serviceList with avaialable services
            foreach(var serviceName in servicesQuryable["offers"].Children<JProperty>())
            {
                Service service = new Service();
                service.Name=serviceName.Name;
                serviceList.services.Add(service);
            }


            return View(serviceList);
        }

        public IActionResult Regions(string Name)
        {

            Regions regions = new Regions();
            IList<string> newList= new List<string>();
            regions.regionCode = newList;

            //gets regions a service is available in from API
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            var regionList = webClient.DownloadString("https://pricing.us-east-1.amazonaws.com/offers/v1.0/aws/"+Name+"/current/region_index.json");
            JObject regionsQuryable = JObject.Parse(regionList);


            //formats date of posting for use in request URL
            regions.service=Name;
            string temp = ((DateTime)regionsQuryable["publicationDate"]).ToString("yyyy-MM-dd HH:mm:ss");
            regions.timeStamp= Regex.Replace(temp, "[^0-9.]", "");
           
            //populates regions list
            foreach (var serviceName in regionsQuryable["regions"].Children<JProperty>())
            {
                regions.regionCode.Add(serviceName.Name);
            }

            return View(regions);
        }

        public IActionResult Pricing(string service, string timeStamp, string item)
        {

            Products products = new Products();
            IList<Product> newList = new List<Product>();
            products.product = newList;
            products.service = service;
            products.region = item;
            
            //retrieves descriptions andf  prices for individual products offered within an AWs service 
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";

            string url = "https://pricing.us-east-1.amazonaws.com/offers/v1.0/aws/" + service + "/" + timeStamp + "/" + item + "/index.json";
            var productList = webClient.DownloadString(url);
            JObject productQuryable = JObject.Parse(productList);


            //populates Product List
            foreach (var productName in productQuryable["products"].Children<JProperty>())
            {
                foreach(var temp in productQuryable["terms"]["OnDemand"][productName.Name].Children<JProperty>())
                {
                    foreach (var temp2 in productQuryable["terms"]["OnDemand"][productName.Name][temp.Name]["priceDimensions"].Children<JProperty>())
                    {
                        Product product = new Product();
                        product.Description = (string)productQuryable["terms"]["OnDemand"][productName.Name][temp.Name]["priceDimensions"][temp2.Name]["description"];
                        product.Price = (decimal)productQuryable["terms"]["OnDemand"][productName.Name][temp.Name]["priceDimensions"][temp2.Name]["pricePerUnit"]["USD"];
                        products.product.Add(product);
                    }
                }
     
            }

            return View(products);
        }
    }
}
