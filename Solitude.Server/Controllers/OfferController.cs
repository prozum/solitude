using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model;
using System;

namespace Solitude.Server
{
    public class OfferController : SolitudeController
    {
        public async Task<IHttpActionResult> Get()
        {
            var uid = new Guid(User.Identity.GetUserId());

            await DB.MatchUser(uid);

            var offers = await DB.GetOffers(uid);

            return Ok(offers);
        }
            
        public async Task<IHttpActionResult> Post(Reply reply)
        {
            var success = await DB.ReplyOffer(new Guid(User.Identity.GetUserId()), reply.EventId, reply.Value);

            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}