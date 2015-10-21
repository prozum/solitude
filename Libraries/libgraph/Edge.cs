using System;

namespace libgraph
{
	public class Edge
	{
		Vertex node1;
		Vertex node2;

		public readonly string Attribute;

		public Edge (Vertex node1, Vertex node2, string a)
		{
			this.node1 = node1;
			this.node2 = node2;
			Attribute = a;
		}
	}
}

