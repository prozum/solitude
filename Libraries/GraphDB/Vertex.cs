using System;
using System.Collections.Generic;

namespace GraphDB
{
	public class Vertex
	{
		// global id counter to make sure every vertex is a special little snowflake
		static int _id = 0;

		int id;

		public int ID
		{
			get
			{
				return id;
			}
		}

		public List<Edge> Edges = new List<Edge>();

		public Vertex ()
		{
			id = _id++;
		}

		public int Degree
		{
			get
			{
				// the degree of a vertex is the number of edges
				return Edges.Count;
			}
		}

		public override bool Equals (object obj)
		{
			return ID == ((Vertex)obj).ID;
		}

		public override int GetHashCode ()
		{
			return ID.GetHashCode ();
		}
	}
}