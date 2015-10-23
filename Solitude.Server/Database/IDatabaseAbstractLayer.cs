using System;
using System.Linq;
using System.Collections.Generic;

namespace Solitude.Server
{
	public interface IDatabaseAbstractLayer
	{
		IEnumerable<User> GetUsers ();
		IEnumerable<Event> GetEvents ();
		IEnumerable<Info> GetInformation ();
	}
}

