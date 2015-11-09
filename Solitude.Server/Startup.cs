using Dal;
using Model;
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

            DatabaseAbstrationLayer dal = new DatabaseAbstrationLayer(gc);

            app.CreatePerOwinContext(() => {
                return dal;
            });

            app.CreatePerOwinContext<SolitudeUserManager>(SolitudeUserManager.Create);
            app.CreatePerOwinContext<SolitudeSignInManager>(SolitudeSignInManager.Create);

            var interests = Enum.GetValues(typeof(Interest));
            var languages = Enum.GetValues(typeof(Language));
            var foodhabits = Enum.GetValues(typeof(Interest));

            foreach (var i in interests)
            {
                dal.AddInterest((Interest)i);
            }

            foreach (var l in languages)
            {
                dal.AddLanguage((Language)l);
            }

            foreach (var f in foodhabits)
            {
                dal.AddFoodHabit((FoodHabit)f);
            }
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