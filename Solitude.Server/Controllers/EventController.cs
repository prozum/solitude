﻿using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Model;

namespace Solitude.Server
{
    [RoutePrefix("api/event")]
    public class EventController : SolitudeController
	{
        public IHttpActionResult Get()
        {
            List<Event> OrderList = new List<Event> 
                {
                    new Event {Title = "Julefrokost", Id = 666, Address = "Everywhere", Description = "Julefrokost", Date = "21-12-12" },
                    new Event {Title = "I-dag", Id = 667, Address = "DE-club", Description = "DE-club", Date = "Thursday" },
                    new Event {Title = "FLAN", Id = 668, Address = "Cassiopeia", Description = "Cassiopeia", Date = "00-00-99" },
                    new Event {Title = "J-dag", Id = 669, Address = "D-building", Description = "J-dag", Date = "05-11-15"},
                    new Event {Title = "ØL", Id = 670, Address = "Cantina", Description = "Free beer", Date = "Always tomorrow"}
                };

            return Ok(OrderList);
		}

        [Authorize]
        public IHttpActionResult Get (int id)
		{
            return Ok(new Event() { Id = id, Date = "Already over", Address = "Cassiopeia", Description = "FLAN party in Cassiopeia"});
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