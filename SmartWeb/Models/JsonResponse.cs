using SmartWeb.Controllers.apiControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWeb.Models
{
    public class JsonResponse
    {
		public ResponseTypes Type { get; set; }
		public object ResponseObj { get; set; }
		public JsonResponse(ResponseTypes type, object response)
		{
			Type = type;
			ResponseObj = response;
		}
    }
}
