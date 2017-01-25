using System.Web.Http;
using System.Threading.Tasks;
using BBBClasses;
using System;

namespace Solitude.Server
{
    public class ReviewController : BBBController
    {
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var events = await DB.GetBeerReviews(id);

            return Ok(events);
        }

		public async Task<IHttpActionResult> Post(Review review)
		{
			//Add the actual review to the database
			review.UserId = UserId;
			await DB.AddBeerReview(review);

			//Add a notification for the brewer --> could be put on a separate thread
			var beer = await DB.GetBeer(review.BeerId);
			Notification n = new Notification();
			n.Data = new string[1]
			{
				BBBNotificationStrings.REVIEW_NOTIFICATION(beer)
			};
			await DB.AddNotification(beer.UserId, n);

            return Ok();
        }
    }
}

