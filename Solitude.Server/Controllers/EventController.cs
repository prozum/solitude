using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Solitude.Server
{
    public class EventController : SolitudeController
    {
        public async Task<IHttpActionResult> Get()
        {
            var events = await DB.GetAttendingEvents(User.Identity.GetUserId());
            return Ok(events);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            await DB.CancelRegistration(User.Identity.GetUserId(), id);
            return Ok();
        }
    }
}

