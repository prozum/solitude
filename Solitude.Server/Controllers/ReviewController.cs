using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model;
using System;

namespace Solitude.Server
{
    public class ReviewController : SolitudeController
    {
        public async Task<IHttpActionResult> Post(Review review)
        {
            review.UserId = new Guid(User.Identity.GetUserId());
            
            await DB.AddReview(review);

            return Ok();
        }
    }
}

