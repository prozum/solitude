using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Owin;
using Neo4j.AspNet.Identity;
using Microsoft.Owin.Host.SystemWeb;

namespace Solitude.Server
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
	{
        private Neo4jUserManager manager;

        public UserController() {}
        public UserController(Neo4jUserManager manager)
		{
            this.Manager = manager;
		}

        public Neo4jUserManager Manager
        {
            get
            {
                return manager ?? Request.GetOwinContext().GetUserManager<Neo4jUserManager>();
            }
            private set
            {
                manager = value;
            }
        }

		[AllowAnonymous]
		[Route("register")]
		public async Task<IHttpActionResult> Register(UserModel userModel)
		{
            var user = new ApplicationUser() { UserName = userModel.UserName };

            IdentityResult result = await Manager.CreateAsync(user, userModel.Password);
			//IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
                
            return Ok();
		}

        [Route("login")]
        public async Task<IHttpActionResult> Get (UserModel user)
        {
            //var result = await _repo.FindUser(user.UserName, user.Password);

            return null;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
	}
}

