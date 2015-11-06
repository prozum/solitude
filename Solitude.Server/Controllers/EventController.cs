using System.Collections.Generic;
using System.Web.Http;
using Dal;

namespace Solitude.Server
{
	public class EventController : ApiController
	{
        public IHttpActionResult Get ()
        {
            List<Event> OrderList = new List<Event> 
                {
                    new Event {ID = 666, Address = "Everywhere", Description = "Julefrokost", Date = "21-12-12" },
                    new Event {ID = 667, Address = "DE-club", Description = "DE-club", Date = "thursday" },
                    new Event {ID = 668, Address = "Cassiopeia", Description = "Cassiopeia", Date = "00-00-00" },
                    new Event {ID = 669, Address = "D-building", Description = "J-dag", Date = "05-11-15"},
                    new Event {ID = 670, Address = "Cantina", Description = "Free beer", Date = "Always tomorrow"}
                };

            return Ok(OrderList);
		}

        [Authorize]
        public IHttpActionResult Get (int id)
		{
            return Ok(new Event() { ID = id, Date = "already over", Address = "Cassiopeia", Description = "FLAN party in Cassiopeia"});
		}
	}
}