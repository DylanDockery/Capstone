using Microsoft.AspNetCore.Mvc;
using Capstone3.Models;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Net;
using System.Security.Cryptography.Xml;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using System.Net.Http;

namespace Capstone3.Controllers
{
    public class SteamController : Controller
    {
 

        public IActionResult Index(string query)
        {
            //Retrieves list of all available applications and add on from stem store
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            var json = webClient.DownloadString("http://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json");

            //parse json string into  app ids and names
            Regex regex = new Regex("\"appid\":(.*?),\"");
            MatchCollection matchesIDs = regex.Matches(json);
            Regex regex2 = new Regex("\"name\":\"(.*?)\"}");
            MatchCollection matchesNames = regex2.Matches(json);

            //populate sapplis tinstance with instances of apps that match search criteria
            Apps appsList = new Apps();
            appsList.Query = query;

            if(query!=null && query.Length>=5) {
                for (int i = 0; i < matchesIDs.Count; i++)
                {
                    if (matchesNames[i].Groups[1].Value.Contains(query)) {
                        AppData temp = new AppData();
                        temp.appid = matchesIDs[i].Groups[1].Value;
                        temp.name = matchesNames[i].Groups[1].Value;
                        appsList.apps.Add(temp);
                    }
                }
            }

            return View(appsList);
        }
        public IActionResult Price(string appid)
        {
            //retrieves price data and conversion rates from APIs
            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            var appInfo = webClient.DownloadString("https://api.steamregionalprices.com/2.0/get/app/" + appid);
            var exchageInfo = webClient.DownloadString("https://openexchangerates.org/api/latest.json?app_id=7fecb2768bb741989cc011acb92755eb&symbols=AUD,RUB,EUR,GBP");

            //populates app instance with prices and region data
            App app = new App();
            app.Prices = new Dictionary<string, Decimal>();
            app.Rates = new Dictionary<string, Decimal>();

            JObject appInfoQuryable = JObject.Parse(appInfo);
            JObject exchangeRate = JObject.Parse(exchageInfo);
            string title = (string)appInfoQuryable["title"];
            string image = (string)appInfoQuryable["imageUrl"];

            app.Name = title.Substring(0, title.Length / 2);
            app.Image = image;

            Regex regex = new Regex("\"regionCode\":\"(.*?)\"");
            MatchCollection matchesRegions = regex.Matches(appInfo);
            Regex regex2 = new Regex("\"price\":(.*?)}");
            MatchCollection matchesPrices = regex2.Matches(appInfo);
            Decimal priceDec = 0m;
            Decimal rateDec = 0m;

            for (int i = 0; i < matchesRegions.Count; i++)
            {
                string price = matchesPrices[i].Groups[1].Value;
                string region = matchesRegions[i].Groups[1].Value;
                if (price.Length != 1)
                {
                    price = price.Trim('\"');
                }
                Decimal.TryParse(price, out priceDec);
                app.Prices.Add(region,priceDec);
            }
            app.Rates.Add("us", 1);
            Decimal.TryParse((string)exchangeRate["rates"]["AUD"], out rateDec);
            app.Rates.Add("au", rateDec);
            Decimal.TryParse((string)exchangeRate["rates"]["EUR"], out rateDec);
            app.Rates.Add("eu1", rateDec);
            Decimal.TryParse((string)exchangeRate["rates"]["EUR"], out rateDec);
            app.Rates.Add("eu2", rateDec);
            Decimal.TryParse((string)exchangeRate["rates"]["GBP"], out rateDec);
            app.Rates.Add("uk", rateDec);
            Decimal.TryParse((string)exchangeRate["rates"]["RUB"], out rateDec);
            app.Rates.Add("ru", rateDec);

            return View(app);
        }
    }
}
