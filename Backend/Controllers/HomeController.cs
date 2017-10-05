using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace vega.Controllers
{
    public class HomeController : Controller
    {
        
        private static string[] Summaries = new[]
        {
            "jow","jow","jow","it works great"
        };

        [HttpGet("[action]")]
        public string[] WeatherForecasts()
        {
            return Summaries;
        }
    }
}
