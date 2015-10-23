using System;
using System.Collections.Generic;
using GraphDB;

namespace Solitude.Server
{
	public interface IClientCommunication
	{
		List<Offer> RequestMatch(User u);
		//n is how many events
		List<Event> GetOwnEvents(int n, bool NEWEST=true);
		void CreateUser (User u);
		void UpdateUser (User u);
		void DeleteUser (User u);
		void UpdateEvent (Event e);
		void DeleteEvent (Event e);
		void ReplyOffer (Offer o, bool a);
		void CancelRegistration (Registration r);
	}
}

