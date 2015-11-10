using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model;

namespace Solitude.Server
{
    [RoutePrefix("api/offer")]
    public class OfferController : SolitudeController
    {
        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            var offers = await DB.GetOffers(User.Identity.GetUserId());
            return Ok(offers);
        }

        [Authorize]
        [Route("reply")]
        public async Task<IHttpActionResult> Reply(Reply reply)
        {
            var success = await DB.ReplyOffer(User.Identity.GetUserId(), reply.Value, reply.EventId);

            return success ? Ok() : BadRequest();
        }
    }
}