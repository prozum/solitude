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

		bool AddCategory (Category c);
		bool AddCategory (Category c, Category sc);
		bool DeleteCategory (Category c);
		bool DeleteCategory (Category c, Category sc);

		bool AddMatch (User u, Match m);
		bool DeleteMatch (Match m);
	}
}

