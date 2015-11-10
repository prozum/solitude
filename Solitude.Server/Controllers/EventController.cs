using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Model;

namespace Solitude.Server
{
    [RoutePrefix("api/event")]
    public class EventController : SolitudeController
	{
        [Authorize]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var events = await DB.GetHostingEvents(User.Identity.GetUserId());
            return Ok(events);
		}

        [Authorize]
        [Route("add")]
        public async Task<IHttpActionResult> Add(Event e)
        {
            e.UserId = User.Identity.GetUserId();

            await DB.AddEvent(e);

            return Ok(new { Id = e.Id});
        }

        [Authorize]
        [Route("delete")]
        public async Task<IHttpActionResult> Delete(int eventId)
        {
            await DB.DeleteEvent(eventId);

            return Ok();
        }

        [Authorize]
        [Route("update")]
        public async Task<IHttpActionResult> Update(Event e)
        {
            await DB.UpdateEvent(e);

            return Ok();
        }
	}
}