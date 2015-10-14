using System;

namespace libgraph
{
	public class Edge
	{
		Vertex node1;
		Vertex node2;

		public Edge (Vertex node1, Vertex node2)
		{
			this.node1 = node1;
			this.node2 = node2;
		}
	}
}

