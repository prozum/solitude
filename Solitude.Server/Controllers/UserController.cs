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
        private SolitudeUserManager _manager;

        public UserController() {}
        public UserController(SolitudeUserManager manager)
        {
            Manager = manager;
        }

        public SolitudeUserManager Manager
        {
            get
            {
                return _manager ?? Request.GetOwinContext().GetUserManager<SolitudeUserManager>();
            }
            private set
            {
                _manager = value;
            }
        }

        public async Task<IHttpActionResult> Get()
        {
            var data = await DB.GetUserData(User.Identity.GetUserId());

            return Ok(data);
        }

        [AllowAnonymous]
        [Route("api/user/check/username/{username}")]
        public async Task<IHttpActionResult> Get(string username)
        {
            var user = await Manager.FindByNameAsync(username);
            return Ok(user != null);
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
                    UserName = userModel.Username, 
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

