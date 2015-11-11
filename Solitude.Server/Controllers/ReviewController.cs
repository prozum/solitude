using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model;

namespace Solitude.Server
{
    public class ReviewController : SolitudeController
    {
        public async Task<IHttpActionResult> Post(Review review)
        {
            review.UserId = User.Identity.GetUserId();
            
            await DB.AddReview(review);

            return Ok();
        }
    }
}

