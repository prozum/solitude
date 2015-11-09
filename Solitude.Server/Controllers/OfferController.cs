using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

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
    }
}