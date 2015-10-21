using System;
using System.Collections.Generic;

namespace libgraph
{
	public class UndirectedGraph
	{
		List<Vertex> Nodes = new List<Vertex>();

		public UndirectedGraph ()
		{
			Nodes.Add (new Vertex());
		}
	}
}

