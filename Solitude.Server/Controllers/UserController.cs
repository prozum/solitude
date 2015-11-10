using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Neo4j.AspNet.Identity;
using Neo4jClient;
using Model;

namespace Solitude.Server
{
    [RoutePrefix("api/user")]
    public class UserController : SolitudeController
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

        [Authorize]
        [Route("event")]
        public async Task<IHttpActionResult> Get()
        {
            var events = await DB.GetAttendingEvents(User.Identity.GetUserId());
            return Ok(events);
        }

        [Authorize]
        [Route("event/cancel/")]
        public async Task<IHttpActionResult> Get(int id)
        {
            await DB.CancelRegistration(User.Identity.GetUserId(), id);
            return Ok();
        }
	}
}

