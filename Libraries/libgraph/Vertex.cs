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
	}
}

