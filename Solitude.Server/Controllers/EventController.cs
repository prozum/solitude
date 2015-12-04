using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System;

namespace Solitude.Server
{
    public class EventController : SolitudeController
    {
        public async Task<IHttpActionResult> Get()
        {
            var events = await DB.GetAttendingEvents(UserId);
            return Ok(events);
        }

        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await DB.CancelEventRegistration(UserId, id);
            return Ok();
        }
    }
}

