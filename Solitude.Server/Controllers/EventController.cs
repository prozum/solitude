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
        public async Task<IHttpActionResult> Get()
        {
            var offers = await DB.GetEvents(User.Identity.GetUserId());
            return Ok(offers);
		}

        [Authorize]
        public IHttpActionResult Get(int id)
		{
            List<Event> OrderList = new List<Event> 
                {
                    new Event {Title = "Julefrokost", Id = 0, Address = "Everywhere", Description = "Julefrokost", Date = "21-12-12" },
                    new Event {Title = "I-dag", Id = 1, Address = "DE-club", Description = "DE-club", Date = "Thursday" },
                    new Event {Title = "FLAN", Id = 2, Address = "Cassiopeia", Description = "Cassiopeia", Date = "00-00-99" },
                    new Event {Title = "J-dag", Id = 3, Address = "D-building", Description = "J-dag", Date = "05-11-15"},
                    new Event {Title = "ØL", Id = 4, Address = "Cantina", Description = "Free beer", Date = "Always tomorrow"}
                };

            return Ok(OrderList[id]);
   		}

        [Authorize]
        [Route("add")]
        public async Task<IHttpActionResult> Add(Event e)
        {
            e.UserId = User.Identity.GetUserId();

            await DB.AddEvent(e);

            return Ok();
        }

        [Authorize]
        [Route("delete")]
        public async Task<IHttpActionResult> Delete(int eID)
        {
            await DB.DeleteEvent(eID);

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