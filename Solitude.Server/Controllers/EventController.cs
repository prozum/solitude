using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using Dal;
using Model;

namespace Solitude.Server
{
	public class EventController : ApiController
	{
        public DatabaseAbstrationLayer DB
        {
            get
            {
                return Request.GetOwinContext().Get<DatabaseAbstrationLayer>();
            }
        }

        public IHttpActionResult Get()
        {
            List<Event> OrderList = new List<Event> 
                {
                    new Event {ID = 666, Address = "Everywhere", Description = "Julefrokost", Date = "21-12-12" },
                    new Event {ID = 667, Address = "DE-club", Description = "DE-club", Date = "Thursday" },
                    new Event {ID = 668, Address = "Cassiopeia", Description = "Cassiopeia", Date = "00-00-99" },
                    new Event {ID = 669, Address = "D-building", Description = "J-dag", Date = "05-11-15"},
                    new Event {ID = 670, Address = "Cantina", Description = "Free beer", Date = "Always tomorrow"}
                };

            return Ok(OrderList);
		}

        [Authorize]
        public IHttpActionResult Get (int id)
		{
            return Ok(new Event() { ID = id, Date = "Already over", Address = "Cassiopeia", Description = "FLAN party in Cassiopeia"});
		}

        [Authorize]
        [Route("add")]
        public async Task<IHttpActionResult> Add(Event e)
        {
            e.UserID = User.Identity.GetUserId();

            await DB.AddEvent(e);

            return Ok();
        }

        [Authorize]
        [Route("delete")]
        public async Task<IHttpActionResult> Delete(int eID)
        {
            await DB.DeleteEvent(User.Identity.GetUserId(), eID);

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