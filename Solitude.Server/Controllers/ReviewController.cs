using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model;
using System;

namespace Solitude.Server
{
    public class ReviewController : SolitudeController
    {
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var events = await DB.GetReviews(id);

            return Ok(events);
        }

        public async Task<IHttpActionResult> Post(Review review)
        {
            review.UserId = UserId;
            
            await DB.AddReview(review);

            return Ok();
        }
    }
}

