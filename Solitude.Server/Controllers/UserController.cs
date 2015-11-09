using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Neo4j.AspNet.Identity;
using Neo4jClient;
using Dal;

namespace Solitude.Server
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
	{
        private SolitudeUserManager manager;

        public UserController() {}
        public UserController(SolitudeUserManager manager)
		{
            this.Manager = manager;
		}

        public SolitudeUserManager Manager
        {
            get
            {
                return manager ?? Request.GetOwinContext().GetUserManager<SolitudeUserManager>();
            }
            private set
            {
                manager = value;
            }
        }

        public IGraphClient DB
        {
            get
            {
                return Request.GetOwinContext().Get<GraphClientWrapper>().GraphClient;
            }
        }

		[AllowAnonymous]
		[Route("register")]
		public async Task<IHttpActionResult> Register(User user)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = new ApplicationUser() { UserName = user.UserName };

            var result = await Manager.CreateAsync(appUser, user.Password);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
                
            return Ok();
		}

        [Authorize]
        [Route("delete")]
        public async Task<IHttpActionResult> Delete()
        {
            var user = await Manager.FindByIdAsync(User.Identity.GetUserId());

            var result = await Manager.DeleteAsync(user);

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

