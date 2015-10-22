using System;

namespace libgraph
{
	public class DirectedEdge : Edge
	{
		public DirectedEdge (Vertex nodeFrom, Vertex nodeTo, EdgeAttribute a, int weight)
			: base(nodeTo, nodeFrom, a, weight)
		{
		}
	}
}

