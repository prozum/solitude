using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model;

namespace Solitude.Server
{
    public class OfferController : SolitudeController
    {
        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            var uid = User.Identity.GetUserId();

            await DB.MatchUser(uid);

            var offers = await DB.GetOffers(uid);

            return Ok(offers);
        }

        [Authorize]
        public async Task<IHttpActionResult> Post(Reply reply)
        {
            var success = await DB.ReplyOffer(User.Identity.GetUserId(), reply.Value, reply.EventId);

            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}