using Dal;
using Model;
using System;
using System.Collections.Generic;
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
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // Use json
            config.Formatters.Clear ();
            config.Formatters.Add (new JsonMediaTypeFormatter ());

            // Authorize by default
            config.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(config);
        }

        private void ConfigureNeo4j(IAppBuilder app, bool initiate = true)
        {
            // Create and connect GraphClient for use in dal and identity framework
            var gc = new GraphClient(new Uri(ConfigurationManager.ConnectionStrings["neo4j"].ConnectionString));
            gc.Connect();

            // Create dal and initiate DB
            var dal = new DatabaseAbstrationLayer(gc);
            if (initiate)
                InitiateDB(dal);

            // Create reference to dal in OwinContext, so it can be accessed by Controllers 
            app.CreatePerOwinContext(() => {
                return dal;
            });

            // Create reference to GraphClientWrapper in OwinContext, so it can be accessed by SolitudeUserStore
            app.CreatePerOwinContext(() => {
                return new GraphClientWrapper(gc);
            });
                
            // Create Solitude UserManager and SignInManager to control users
            app.CreatePerOwinContext<SolitudeUserManager>(SolitudeUserManager.Create);
            app.CreatePerOwinContext<SolitudeSignInManager>(SolitudeSignInManager.Create);
        }

        void InitiateDB(DatabaseAbstrationLayer dal)
        {
            // Create/reset Event Id Counter to 0
            dal.SetEventIdCounter(0);

            // Create User InfoTypes
            Language.Get().ForEach((Language l) => dal.AddLanguage(l)); 
            Interest.Get().ForEach((Interest i) => dal.AddInterest(i)); 
            FoodHabit.Get().ForEach((FoodHabit f) => dal.AddFoodHabit(f)); 
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