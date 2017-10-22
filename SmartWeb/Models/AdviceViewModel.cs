using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWeb.Models
{
	public class Slip
	{
		public string advice { get; set; }
		public string slip_id { get; set; }
	}

	public class RootAdvice
	{
		public Slip slip { get; set; }
	}
}
