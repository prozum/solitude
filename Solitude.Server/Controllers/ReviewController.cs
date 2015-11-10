using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Dal;
using Model;

namespace Solitude.Server
{
    public class ReviewController : SolitudeController
    {
        [Authorize]
        public async Task<IHttpActionResult> Post(Review review)
        {
            review.UserId = User.Identity.GetUserId();
            
            await DB.AddReview(review);

            return Ok();
        }
    }
}

