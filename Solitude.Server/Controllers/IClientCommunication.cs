using System;
using System.Collections.Generic;

namespace Solitude.Server
{
	public interface IClientCommunication
	{
		List<Offer> RequestMatch();
		List<Event> GetOwnEvents(int n, bool NEWEST=true);

	}
}

