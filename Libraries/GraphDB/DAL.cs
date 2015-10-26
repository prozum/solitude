using System;
using System.Collections.Generic;

namespace GraphDB
{
	public class DAL : IDAL
	{
		public DAL () 
		{
			
		}

		public IEnumerable<User> GetUsers ()
		{
			throw new NotImplementedException ();
		}

		public IEnumerable<Event> GetEvents ()
		{
			throw new NotImplementedException ();
		}
	
		public IEnumerable<CategoryVertex> GetCategories ()
		{
			throw new NotImplementedException ();
		}

		public bool AddUser (User u)
		{
			throw new NotImplementedException ();
		}

		public bool DeleteUser (User u)
		{
			throw new NotImplementedException ();
		}

		public bool UpdateUser ()
		{
			throw new NotImplementedException ();
		}

		public bool AddEvent (Event e)
		{
			throw new NotImplementedException ();
		}

		public bool DeleteEvent (Event e)
		{
			throw new NotImplementedException ();
		}

		public bool UpdateEvent (Event e)
		{
			throw new NotImplementedException ();
		}

		public bool AddCategory (Graph g, Category c)
		{
			throw new NotImplementedException ();
		}

		public bool AddCategory (Graph g, Category c, Category sc)
		{
			throw new NotImplementedException ();
		}

		public bool DeleteCategory (Graph g, Category c)
		{
			throw new NotImplementedException ();
		}

		public bool AddMatch (User u, Match m)
		{
			throw new NotImplementedException ();
		}

		public bool DeleteMatch (Match m)
		{
			throw new NotImplementedException ();
		}
	}
}

