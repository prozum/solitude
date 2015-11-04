﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Owin.Security.AesDataProtectorProvider;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Cookies;
using System.Web.Http;
using System.Web.Configuration;
using System.Net.Http.Formatting;
using Neo4jClient;
using Neo4j.AspNet.Identity;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;

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
            app.CreatePerOwinContext(() => {
                var gc = new GraphClient(new Uri(ConfigurationManager.ConnectionStrings["neo4j"].ConnectionString));
                DAL.DAL.client = gc;
                gc.Connect();
                var gcw = new GraphClientWrapper(gc);
                return gcw;
            });

            app.CreatePerOwinContext<Neo4jUserManager>(Neo4jUserManager.Create);
            app.CreatePerOwinContext<Neo4jSignInManager>(Neo4jSignInManager.Create);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // DpapiDataProtector is not supported on Linux/OSX
            // Use UseAesDataProtector instead
            app.UseAesDataProtectorProvider();

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
                {
                    Provider = new Neo4jAuthorizationServerProvider(),
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/api/token"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}