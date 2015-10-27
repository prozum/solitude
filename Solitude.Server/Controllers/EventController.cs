using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;

namespace Solitude.Server
{
	public class EventController : ApiController
	{
		public IEnumerable<EventModel> Get ()
		{
			return null;
		}

		public EventModel Get (int id)
		{
            return new EventModel() { ID = id, Name = "FLAN", Description = "Lan party in Cassiopeia"};
		}
	}
}