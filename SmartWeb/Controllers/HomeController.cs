using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartWeb.Models;
using SmartWeb.Services;

namespace SmartWeb.Controllers
{
    public class HomeController : Controller
    {
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(InputViewModel model)
		{
			var text = model.Text;
			// gets the weather in a requested city
			if(text.Contains("weather"))
			{
				return RedirectToAction("Weather", new { city = model.Text.Split(" ").Last() });
			}
			// simple calculator, only addition works 
			else if(text.Split('+').Length == 2)
			{
				var nums = text.Split('+');
				int s1, s2;
				if(Int32.TryParse(nums[1],out  s1) && Int32.TryParse(nums[0], out  s2))
				{
					return RedirectToAction("Calculator", new { result = (s1 + s2).ToString() });
				}
			}
			
			else if(text.StartsWith("download"))
			{
				var result = text.Split(" ").Skip(1);
				if(result.Count()>0)
				{
					return RedirectToAction("Download", new {apps  = result});
				}
			}
			else if(text.StartsWith("flights"))
			{
				var parameters = text.Split(" ").Skip(1);

				return RedirectToAction("Flights", new { parameters = parameters });
			}
			else if(text.Contains("motivate"))
			{
				return RedirectToAction("Calculator");
			}
			
		return RedirectToAction("Calculator", new { text = "Something went wrong"});
			
		}
		
		public IActionResult Weather(string city)
		{
			var service = new WeatherService();
			var result = service.GetWeather(city).Result;
			return View(result);
		}

		public IActionResult Calculator(string result)
		{
			return View(result);
		}

		public IActionResult Download(string[] apps)
		{
			var result = new DownloadViewModel { Link = $"https://ninite.com/{apps.Aggregate((a, b) => a + "-" + b)}/ninite.exe" };
			return View("Download", result);
		}


		public IActionResult Flights(string[] parameters)
		{
			var service = new SkyService();
			var result = service.GetList(parameters[0], parameters[1], parameters[2], parameters[3]).Result;
			return View(result);
		}
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

	
    }
}
