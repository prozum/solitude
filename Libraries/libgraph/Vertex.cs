using System;
using System.Collections.Generic;

namespace libgraph
{
	public class Vertex
	{
		// global id counter
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
				return Edges.Count;
			}
		}

		public void Connect (Vertex n, string a)
		{
			Edges.Add (new Edge (this, n, a));
			n.Edges.Add (new Edge (n, this, a));
		}
	}
}

