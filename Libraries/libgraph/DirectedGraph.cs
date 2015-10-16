using System;
using System.Collections.Generic;

namespace libgraph
{
	public class DirectedGraph
	{
		List<Vertex> Nodes = new List<Vertex>();

		public DirectedGraph ()
		{
			Nodes.Add (new Vertex());
		}

		/*
		public void AddVertex(Vertex Node)
		{

		}
		*/

		public Edge ConnectVertices (Vertex NodeFrom, Vertex NodeTo)
		{
			return new Edge (NodeFrom, NodeTo);
		}
	}
}

