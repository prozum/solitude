using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

namespace Solitude.Server
{
    public class UserController : ApiController
	{
		private AuthRepository _repo = null;

		public UserController()
		{
			_repo = new AuthRepository();
		}

		[AllowAnonymous]
		[Route("register")]
		public async Task<IHttpActionResult> Register(UserModel userModel)
		{
			IdentityResult result = await _repo.RegisterUser(userModel);

			return null;
		}

        [Route("login")]
        public async Task<IHttpActionResult> Get (UserModel user)
        {
            var result = await _repo.FindUser(user.UserName, user.Password);

            return null;
        }
	}
}

