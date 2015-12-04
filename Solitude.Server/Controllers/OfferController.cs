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
            await DB.MatchUser(UserId);

            var offers = await DB.GetOffers(UserId);

            return Ok(offers);
        }
            
        public async Task<IHttpActionResult> Post(Guid id)
        {
            var success = await DB.AcceptOffer(UserId, id);

            if (!success)
                return BadRequest();

            return Ok();
        }

        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await DB.DeclineOffer(UserId, id);

            return Ok();
        }
    }
}