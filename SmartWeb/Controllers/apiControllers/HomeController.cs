using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartWeb.Services;
using SmartWeb.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SmartWeb.Controllers.apiControllers
{
	[Produces("application/json")]
	[Route("api/Home")]
	public class HomeController : Controller
	{
		[HttpGet]
		public async Task<JsonResult> Index(string input)
		{
			var text = input;
			// gets the weather in a requested city
			if (text.Contains("weather"))
			{
				var service = new WeatherService();
				var result = service.GetWeather(text.Split(" ").Last()).Result;
				return new JsonResult(new JsonResponse(ResponseTypes.Weather, result));
			}
			// simple calculator, only addition works 
			else if (text.Split('+').Length == 2)
			{
				var nums = text.Split('+');
				int s1, s2;
				if (Int32.TryParse(nums[1], out s1) && Int32.TryParse(nums[0], out s2))
				{
					return new JsonResult(new JsonResponse(ResponseTypes.Calculator, s1 + s2));
				}
			}

			else if (text.StartsWith("download"))
			{
				var result = text.Split(" ").Skip(1);
				if (result.Count() > 0)
				{
					var link = new DownloadViewModel { Link = $"https://ninite.com/{result.Aggregate((a, b) => a + "-" + b)}/ninite.exe" };
					return new JsonResult(new JsonResponse(ResponseTypes.Download, link));
				}
			}
			else if (text.StartsWith("flight"))
			{
				var parameters = text.Split(" ").Skip(1).ToList();
				var service = new SkyService();
				var result = service.GetList(parameters[0].Trim(), parameters[1].Trim(),parameters[2].Trim(),parameters[3].Trim()).Result;
				if (result.Count == 0)
				{
					var resultObj = new SkyScannerViewModel
					{
						Agency = "Wizz Air",
						Currency = "HUF",
						DepartureDate = "2017-11-11",
						OutboundDate = "2017-11-12",
						DestinationCode = "BUD",
						FromCode = "VLC",
						Price = "3000"
					};
					var resultObj2 = new SkyScannerViewModel
					{
						Agency = "Wizz Air",
						Currency = "HUF",
						DepartureDate = "2017-11-11",
						OutboundDate = "2017-11-12",
						DestinationCode = "BUD",
						FromCode = "VLC",
						Price = "3000"
					};
					var list = new List<SkyScannerViewModel>();
					list.Add(resultObj);
					list.Add(resultObj2);
					return new JsonResult(new JsonResponse(ResponseTypes.Flight, list));
				}
				return new JsonResult(new JsonResponse(ResponseTypes.Flight, result));
			}
			else if (text.Contains("motivate"))
			{
				using (HttpClient client = new HttpClient())
				using (HttpResponseMessage response = await client.GetAsync("http://api.adviceslip.com/advice"))
				using (HttpContent content = response.Content)
				{
					// ... Read the string.
					string result = await content.ReadAsStringAsync();
					var json = JsonConvert.DeserializeObject<RootAdvice>(result);
					var advice = json.slip.advice;
					return new JsonResult(new JsonResponse(ResponseTypes.Advice, advice));
				}
			}
			
			return new JsonResult(new JsonResponse(ResponseTypes.Error, "Something went wrong"));
		}
	}
}