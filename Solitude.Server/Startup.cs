using Dal;
using System;
using System.Configuration;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Net.Http.Formatting;
using Neo4jClient;
using Neo4j.AspNet.Identity;

namespace Solitude.Server
{
    public class Startup
    {
        public void Configuration (IAppBuilder app)
        {
            ConfigureNeo4j(app);
            ConfigureOAuth(app);
            ConfigureWebApi(app);
        }

        private void ConfigureWebApi (IAppBuilder app)
        {
            var config = new HttpConfiguration ();
            config.MapHttpAttributeRoutes ();
            config.Routes.MapHttpRoute (
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            // Use json
            config.Formatters.Clear ();
            config.Formatters.Add (new JsonMediaTypeFormatter ());

            app.UseWebApi(config);
        }

        private void ConfigureNeo4j(IAppBuilder app)
        {
            var gc = new GraphClient(new Uri(ConfigurationManager.ConnectionStrings["neo4j"].ConnectionString));
            gc.Connect();

            app.CreatePerOwinContext(() => {
                return new GraphClientWrapper(gc);
            });

            app.CreatePerOwinContext(() => {
                return new DatabaseAbstrationLayer(gc);
            });

            app.CreatePerOwinContext<SolitudeUserManager>(SolitudeUserManager.Create);
            app.CreatePerOwinContext<SolitudeSignInManager>(SolitudeSignInManager.Create);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // DpapiDataProtector is not supported on Linux/OSX
            // Use UseAesDataProtector instead
            app.UseAesDataProtectorProvider();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
                {
                    Provider = new SolitudeAuthorizationServerProvider(),
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/api/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}