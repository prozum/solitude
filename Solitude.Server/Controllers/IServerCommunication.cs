using System;
using System.Collections.Generic;

namespace Solitude.Server
{
	public interface IServerCommunication : IClientCommunication
	{
		List<Offer> SendMatch ();
		List<Event> SendOwnEvents ();
	}
}

