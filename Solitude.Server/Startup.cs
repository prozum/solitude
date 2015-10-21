using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin.Security.Facebook;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace Solitude.Server
{
	public class Startup
	{
		public void Configuration (IAppBuilder app)
		{
			var webApiConfiguration = ConfigureWebApi ();
			app.UseWebApi (webApiConfiguration);
			app.UseFacebookAuthentication ("1654758468126707", "App Secret");
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