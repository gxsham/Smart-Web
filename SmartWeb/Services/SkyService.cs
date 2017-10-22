using Newtonsoft.Json;
using SmartWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartWeb.Services
{
	public class SkyService
	{
		private readonly string _apiKey = "ha712564909427747796218296388326";
		private readonly string _endpoint = @"http://partners.api.skyscanner.net/apiservices/browsequotes/v1.0/";

		public async Task<List<SkyScannerViewModel>> GetList(string from, string to, string start, string end)
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var link = $"{_endpoint}HU/HUF/USD/{from}/{to}/{start}/{end}?apiKey={_apiKey}";
				using (HttpResponseMessage response = await client.GetAsync(link))
				{
					using (HttpContent content = response.Content)
					{
						var jsonResult = await content.ReadAsStringAsync();
						var json = JsonConvert.DeserializeObject<RootObjects>(jsonResult);
						var agencies = json.Carriers;
						var places = json.Places;
						var resultList = new List<SkyScannerViewModel>();
						var currency = "HUF";
						foreach (var item in json.Quotes)
						{
							resultList.Add(
								new SkyScannerViewModel
								{
									Agency = agencies.First(x => x.CarrierId == item.InboundLeg.CarrierIds.First()).Name,
									Currency = currency,
									DepartureDate = item.OutboundLeg.DepartureDate.ToString("MMMMM dd yyyy"),
									OutboundDate = item.InboundLeg.DepartureDate.ToString("MMMMM dd yyyy"),
									DestinationCode = $"{places.First(x => x.PlaceId == item.OutboundLeg.OriginId).IataCode} {places.First(x => x.PlaceId == item.InboundLeg.OriginId).IataCode}",
									FromCode = $"{places.First(x => x.PlaceId == item.InboundLeg.OriginId).IataCode} {places.First(x => x.PlaceId == item.OutboundLeg.OriginId).IataCode}",
									Price = item.MinPrice.ToString()
								}
							);
						}
						return resultList;
					}
				}
			}
		}

		public async Task<List<SkySurpriseViewModel>> GetRandom()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				using (HttpResponseMessage response = await client.GetAsync($"{_endpoint}HU/HUF/USD/BUD/anywhere/anytime/anytime?apiKey={_apiKey}"))
				{
					using (HttpContent content = response.Content)
					{
						var jsonResult = await content.ReadAsStringAsync();
						var json = JsonConvert.DeserializeObject<RootObjects>(jsonResult);
						var agencies = json.Carriers;
						var places = json.Places;
						var resultList = new List<SkySurpriseViewModel>();
						var currency = "HUF";
						foreach (var item in json.Quotes)
						{
							resultList.Add(
								new SkySurpriseViewModel
								{
									Agency = agencies.First(x => x.CarrierId == item.InboundLeg.CarrierIds.First()).Name,
									Currency = currency,
									DepartureDate = item.OutboundLeg.DepartureDate.ToString("MMMMM dd yyyy"),
									OutboundDate = item.InboundLeg.DepartureDate.ToString("MMMMM dd yyyy"),
									DestinationCode = $"{places.First(x => x.PlaceId == item.OutboundLeg.OriginId).IataCode} {places.First(x => x.PlaceId == item.InboundLeg.OriginId).IataCode}",
									FromCode = $"{places.First(x => x.PlaceId == item.InboundLeg.OriginId).IataCode} {places.First(x => x.PlaceId == item.OutboundLeg.OriginId).IataCode}",
									Price = (int)long.Parse(item.MinPrice)
								}
							);
						}
						return resultList;
					}
				}
			}
		}
	}
}
