using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphDB
{
	public class DAL : IDAL
	{
		Graph graph;

		public DAL (Graph g) 
		{
			graph = g;
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

		public bool AddCategory (Category c)
		{
			throw new NotImplementedException ();
		}

		public bool AddCategory (Category c, Category sc)
		{
			throw new NotImplementedException ();
		}

		public bool DeleteCategory (Category c)
		{
			throw new NotImplementedException ();
		}

		public bool DeleteCategory (Category c, Category sc)
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
	
