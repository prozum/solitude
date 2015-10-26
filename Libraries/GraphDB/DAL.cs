using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphDB
{
	public class UserEnum : IEnumerator
	{
		public UserEnum()
		{
			
		}
	}

	public class DAL : IDAL
	{
		Graph graph;

		public DAL (Graph g) 
		{
			graph = g;
		}

		public IEnumerable<User> GetUsers ()
		{
			List<Edge> e = graph.SearchCategory (Category.USERS).Edges;
			List<User> u = new List<User> ();

			foreach (var edge in e)
			{
				u.Add(edge.Node2);
			}

			return u.GetEnumerator ();
		}

		public IEnumerable<Event> GetEvents ()
		{
			List<Edge> e = graph.SearchCategory (Category.EVENT).Edges;
			List<Event> u = new List<Event> ();

			foreach (var edge in e)
			{
				u.Add (edge.Node2);
			}

		}
	
		public IEnumerable<CategoryVertex> GetCategories ()
		{
			return graph.Categories.GetEnumerator ();
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
	
