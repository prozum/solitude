using System;
using System.Collections.Generic;

namespace Application
{
	class Event
	{
		public Event()
		{
			
		}
	}

	class User
	{
		public User()
		{

		}
	}

	class Guest : User
	{
		public Guest()
		{

		}
	}

	class Host : User
	{
		public Host()
		{

		}
	}

	struct InfoChange
	{
		string key, value;
	}

	public interface IMatch
	{
		List<Event> findMatch (Guest g);
	}

	public interface IClientCommunication
	{
		List<Event> requestMatch ();
		List<Event> getOwnEvents (int n, bool NEWEST = true);
		void createUser ();
		void updateUser (InfoChange i);
		void deleteUser (User u);
		void createEvent ();
		void updateEvent ();
		void deleteEvent ();
		void replyOffer (bool a);
		void cancelReg (Event e);
	}

	public interface IServerCommunication : IClientCommunication
	{
		List<Event> sendMatch ();
		List<Event> sendOwnEvents ();
	}

	public interface IDatabaseAbstractLayer
	{
		//IEnumerable Get(Delegate d);
		IEnumerable<User> getUsers ();
		IEnumerable<Event> getEvents ();
		IEnumerable<Events> getOpenEvents ();
	}
}