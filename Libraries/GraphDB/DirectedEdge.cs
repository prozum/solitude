using System;

namespace GraphDB
{
	public class DirectedEdge : Edge
	{
		public DirectedEdge (Vertex nodeFrom, Vertex nodeTo, EdgeAttribute a, int weight)
			: base(nodeTo, nodeFrom, a, weight)
		{
		}
	}
}