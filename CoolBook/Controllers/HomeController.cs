using CoolBook.Models;
using CoolBook.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoolBook.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly CoolBookContext _context;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(CoolBookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IEnumerable<Store> GetStores()
        {
            return _context.Store.ToList();
        }

        [HttpGet]
        public async Task<string> GetTemprature(double lon, double lat)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var tempUrl = $"http://api.openweathermap.org/data/2.5/weather?units=metric&lat={lat}&lon={lon}&APPID=7dac995a44b3a78e115eab362ea2857b";
                    var res = await client.GetAsync(tempUrl);
                    res.EnsureSuccessStatusCode();
                    var content = await res.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<JObject>(content);
                    return json?["main"]?["temp"]?.ToString();
                }
                catch
                {
                    throw new Exception("failed getting the weather");
                }
            }
        }
    }
}
