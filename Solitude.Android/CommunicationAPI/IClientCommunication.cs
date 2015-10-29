using System;
using System.Collections.Generic;

using DineWithaDane.Android;
using System.Threading.Tasks;

namespace ClientCommunication
{
	public interface IClientCommunication
	{
		Task<List<Offer>> RequestOffers ();
		Task<List<Event>> GetOwnEvents (int n, bool NEWEST = true);
		void CreateUser (User u);
		void UpdateUser (InfoChange i);
		void DeleteUser ();
		void CreateEvent (Event e);
		void UpdateEvent ();
		void DeleteEvent (Event e);
		void ReplyOffer (bool a);
		void CancelReg (Event e);
	}
}

