using System;
using System.Collections.Generic;

namespace Solitude.Server
{
	public interface IServerCommunication
	{
		// sends offers to the user
		void RequestMatch(User u);

		//sends n events at max back to the user
		void GetOwnEvents(int n, bool NEWEST=true);

		void CreateUser (User u);
		void UpdateUser (User u);
		void DeleteUser (User u);
		void UpdateEvent (Event e);
		void DeleteEvent (Event e);
		void ReplyOffer (Offer o, bool a);
		void CancelRegistration (Registration r);
		void SendMatch (List<Offer> m);
		void SendOwnEvents (List<Event> e);
	}
}

