using System;
using System.Collections.Generic;

namespace GraphDB
{
	public interface IDAL
	{
		IEnumerable<User> GetUsers ();
		IEnumerable<Event> GetEvents ();
		IEnumerable<CategoryVertex> GetCategories ();

		bool AddUser (User u);
		bool DeleteUser (User u);
		bool UpdateUser ();

		bool AddEvent (Event e);
		bool DeleteEvent (Event e);
		bool UpdateEvent (Event e);

		bool AddCategory (Graph g, Category c);
		bool AddCategory (Graph g, Category c, Category sc);
		bool DeleteCategory (Graph g, Category c);

		bool AddMatch (User u, Match m);
		bool DeleteMatch (Match m);
	}
}

