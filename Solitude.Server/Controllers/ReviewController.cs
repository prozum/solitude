using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Dal;

namespace Solitude.Server
{
    [RoutePrefix("api/review")]
    public class ReviewController : ApiController
    {
        public DatabaseAbstrationLayer DB
        {
            get
            {
                return Request.GetOwinContext().Get<DatabaseAbstrationLayer>();
            }
        }

        [Authorize]
        [Route("add")]
        public async Task<IHttpActionResult> Add(ReviewModel model)
        {
            await DB.AddReview(User.Identity.GetUserId(), model.ReviewText, model.Rating);
            return Ok();
        }
    }
}

