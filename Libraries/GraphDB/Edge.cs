using System;

namespace GraphDB
{
	public class Edge
	{
		public readonly Vertex Node1;
		public readonly Vertex Node2;
		public readonly EdgeAttribute Attribute;
		public readonly int Weight;

		public Edge (Vertex node1, Vertex node2, EdgeAttribute a, int weight)
		{
			Attribute = a;
			Weight = weight;
			Node1 = node1;
			Node2 = node2;
		}
	}
}

