using System;
using System.Collections.Generic;

namespace Solitude.Server
{
	public interface IServerCommunication : IClientCommunication
	{
		void SendMatch (List<Offer> m);
		void SendOwnEvents (List<Event> e);
	}
}

