using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Model;
using System;

namespace Solitude.Server
{
    public class HostController : SolitudeController
    {
        public async Task<IHttpActionResult> Get()
        {
            var events = await DB.GetHostingEvents(new Guid(User.Identity.GetUserId()));
            return Ok(events);
        }
            
        public async Task<IHttpActionResult> Post(Event e)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            e.UserId = new Guid(User.Identity.GetUserId());

            await DB.AddEvent(e);

            return Ok(new { Id = e.Id});
        }
            
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await DB.DeleteEvent(id);

            return Ok();
        }
            
        public async Task<IHttpActionResult> Put(Event e)
        {
            await DB.UpdateEvent(e, new Guid(User.Identity.GetUserId()));

            return Ok();
        }
    }
}