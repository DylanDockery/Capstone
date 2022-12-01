using System.Security.Authentication;

namespace Capstone3.Models
{
    public class AppPrice
    {
        public string Name { get; set; }
        public Dictionary<string, Decimal> Prices { get; set; }
        public Dictionary<string, Decimal> Rates{get; set;}

        public string Image { get; set; }

        public Dictionary<string, string> Region = new Dictionary<string, string>
        {   {"au","Austrailia"},
            {"eu1" , "Western Europe"},
            {"eu2", "Eastern Europe" },
            {"us", "United States" },
            {"uk","United Kingdom"},
            {"ru","Russia" }

        };
        public Dictionary<string, string> Currency = new Dictionary<string, string>
        {   {"au","AUD"},
            {"eu1" , "EUR"},
            {"eu2", "EUR" },
            {"us", "USD" },
            {"uk","GBP"},
            {"ru","RUB" }

        };

    }

}
