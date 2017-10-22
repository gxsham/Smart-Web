using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWeb.Models
{
    public class SkySurpriseViewModel
    {
		public string FromCode { get; set; }
		public string DestinationName { get; set; }
		public string DestinationCode { get; set; }
		public int Price { get; set; }
		public string Currency { get; set; }
		public string Agency { get; set; }
		public string DepartureDate { get; set; }
		public string OutboundDate { get; set; }
	}
}
