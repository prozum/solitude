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

		public IEnumerable<Event> Get ()
		{
			return null;
		}

		public Event Get (int id)
		{
            return new Event() { Name = "FLAN", Description = "Lan party in Cassiopeia"};
		}
	}
}