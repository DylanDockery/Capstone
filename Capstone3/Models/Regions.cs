﻿using Microsoft.AspNetCore.Routing;

namespace Capstone3.Models
{
    public class Regions
    {
        public string  service { get; set; }
        public IList<string> regionCode { get; set; }
        public string timeStamp { get; set; }

        public Dictionary<string, string> Region = new Dictionary<string, string>
        {   {"us-east-2","US East (Ohio)"},
            {"us-east-1" , "US East (N. Virginia)"},
            {"us-west-1", "US West (N. California)" },
            {"us-west-2", "US West (Oregon)" },
            {"af-south-1", "Africa (Cape Town)"},
            {"ap-east-1","Asia Pacific (Hong Kong)"},
            {"ap-south-2" , "Asia Pacific (Hyderabad)"},
            {"ap-southeast-3", "Asia Pacific (Jakarta)" },
            {"ap-south-1", "Asia Pacific (Mumbai)" },
            {"ap-northeast-3", "Asia Pacific (Osaka)"},
            {"ap-northeast-2","Asia Pacific (Seoul)"},
            {"ap-southeast-1" , "Asia Pacific (Singapore)"},
            {"ap-southeast-2", "Asia Pacific (Sydney)" },
            {"ap-northeast-1", "Asia Pacific (Tokyo)" },
            {"ca-central-1", "Canada (Central)"},
            {"eu-central-1","Europe (Frankfurt)"},
            {"eu-west-1" , "Europe (Ireland)"},
            {"eu-west-2", "Europe (London)" },
            {"eu-south-1", "Europe (Milan)" },
            {"eu-west-3", "Europe (Paris)"},
            {"eu-south-2","Europe (Spain)"},
            {"eu-north-1" , "Europe (Stockholm)"},
            {"eu-central-2", "Europe (Zurich)" },
            {"me-south-1", "Middle East (Bahrain)" },
            {"me-central-1", "Middle East (UAE)"},
            {"sa-east-1","South America (São Paulo)"},
            {"us-gov-east-1" , "AWS GovCloud (US-East)"},
            {"us-gov-west-1", "AWS GovCloud (US-West)" }

        };
    }
}
