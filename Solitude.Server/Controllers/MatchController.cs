using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Solitude.Server
{
    [RoutePrefix("api/match")]
    public class MatchController : ApiController
    {
        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            var match = await DAL.DAL.GetOffers(User.Identity.GetUserId());
            return Ok(match);
        }
    }
}