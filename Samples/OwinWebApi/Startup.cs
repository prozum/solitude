using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace OwinWebApi
{
	public class Startup
	{
		public void Configuration (IAppBuilder app)
		{
			var webApiConfiguration = ConfigureWebApi ();
			app.UseWebApi (webApiConfiguration);
		}

		private HttpConfiguration ConfigureWebApi ()
		{
			var config = new HttpConfiguration ();
			config.MapHttpAttributeRoutes ();
			config.Routes.MapHttpRoute (
				"DefaultApi",
				"api/{controller}/{id}",
				new { id = RouteParameter.Optional });
			config.Formatters.Clear ();
			config.Formatters.Add (new JsonMediaTypeFormatter ());
			return config;
		}
	}
}