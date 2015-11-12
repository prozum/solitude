using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Model;

namespace Solitude.Server
{
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

        public async Task<IHttpActionResult> Get()
        {
            var data = await DB.GetUserData(User.Identity.GetUserId());

            return Ok(data);
        }

		[AllowAnonymous]
		public async Task<IHttpActionResult> Post(User userModel)
		{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new SolitudeUser() 
                { 
                    Name = userModel.Name, 
                    UserName = userModel.UserName, 
                    Address = userModel.Address,
                    Birthdate = userModel.Birthdate
                };

            var result = await Manager.CreateAsync(user, userModel.Password);

            IHttpActionResult errorResult = ErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
                
            return Ok();
		}
            
        public async Task<IHttpActionResult> Delete()
        {
            var user = await Manager.FindByIdAsync(User.Identity.GetUserId());

            var result = await Manager.DeleteAsync(user);

            IHttpActionResult errorResult = ErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }
	}
}

