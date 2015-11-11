using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Model;

namespace Solitude.Server
{
    public class HostController : SolitudeController
	{
        public async Task<IHttpActionResult> Get()
        {
            var events = await DB.GetHostingEvents(User.Identity.GetUserId());
            return Ok(events);
		}
            
        public async Task<IHttpActionResult> Post(Event e)
        {
            e.UserId = User.Identity.GetUserId();

            await DB.AddEvent(e);

            return Ok(new { Id = e.Id});
        }
            
        public async Task<IHttpActionResult> Delete(int id)
        {
            await DB.DeleteEvent(id);

            return Ok();
        }
            
        public async Task<IHttpActionResult> Put(Event e)
        {
            await DB.UpdateEvent(e);

            return Ok();
        }
	}
}