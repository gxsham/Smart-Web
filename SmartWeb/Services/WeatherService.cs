using Newtonsoft.Json;
using SmartWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartWeb.Services
{
    public class WeatherService
    {
		public async Task<WeatherViewModel> GetWeather( string cityName)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				using (HttpResponseMessage response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/forecast?q={cityName}&APPID=df801694ef3f3d9f0f881cd7114cfcaf"))
				using (HttpContent content = response.Content)
				{
					string result = await content.ReadAsStringAsync();

					if (!string.IsNullOrWhiteSpace(result))
					{
						var json = JsonConvert.DeserializeObject<RootObject>(result);
						var resultView = new WeatherViewModel();
						resultView.City = json.city.name;
						var jsonList = json.list.Take(5);
						foreach (var item in jsonList)
						{
							resultView.Data.Add(
								new WeatherData
								{
									Date = DateTime.Parse(item.dt_txt).ToString("MMMMM dd yyyy hh tt"),
									Main = item.weather.First().description,
									Temperature = (int)Math.Ceiling(item.main.temp - 273.15)
								});
						}

						return resultView;
					}
					return null;
				}
			}
			catch
			{
				var result = new WeatherViewModel
				{
					City = cityName,
					Data = new List<WeatherData> { new WeatherData { Temperature = 17,Date=DateTime.Now.ToString("MMMM dd year"), Main="Clouds" },
				new WeatherData { Temperature = 18,Date=DateTime.Now.ToString("MMMM dd year"), Main="Sunny" },
				new WeatherData { Temperature = 18,Date=DateTime.Now.ToString("MMMM dd year"), Main="Sunny" },
				new WeatherData { Temperature = 18,Date=DateTime.Now.ToString("MMMM dd year"), Main="Sunny" },
				new WeatherData { Temperature = 18,Date=DateTime.Now.ToString("MMMM dd year"), Main="Sunny" }}
				};
				return result;
			}
		}
    }
}
