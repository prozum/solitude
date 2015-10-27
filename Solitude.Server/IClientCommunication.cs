using System;
using System.Collections.Generic;
using GraphDB;

namespace Solitude.Server
{
	public interface IClientCommunication
	{
		List<Offer> RequestMatch(User u);
		//n is how many events
		List<EventModel> GetOwnEvents(int n, bool NEWEST=true);
		void CreateUser (User u);
		void UpdateUser (User u);
		void DeleteUser (User u);
		void UpdateEvent (EventModel e);
		void DeleteEvent (EventModel e);
		void ReplyOffer (Offer o, bool a);
		void CancelRegistration (Registration r);
	}
}

