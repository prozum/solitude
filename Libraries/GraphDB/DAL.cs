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
			/*
			var e = graph.Categories[Category.USERS].Edges;
			var u = new Dictionary<Vertex, Edge> ();

			foreach (var edge in e.Values)
			{
				u.Add(edge.Node2, edge);
			}

			return u.GetEnumerator ();
			*/

			throw new NotImplementedException ();
		}

		public IEnumerable<Event> GetEvents ()
		{
			/*
			var e = graph.Categories[Category.EVENT].Edges;
			var u = new Dictionary<Vertex, Edge> ();

			foreach (var edge in e.Values)
			{
				u.Add (edge.Node2, edge);
			}

			return u.GetEnumerator ();
			*/

			throw new NotImplementedException ();
		}
	
		public IEnumerable<CategoryVertex> GetCategories ()
		{
			//return graph.Categories.GetEnumerator ();

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
	
