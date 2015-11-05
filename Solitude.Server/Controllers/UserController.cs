using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Neo4j.AspNet.Identity;

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

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
                
            return Ok();
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

