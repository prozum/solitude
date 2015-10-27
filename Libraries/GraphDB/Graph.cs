using System;
using System.Collections.Generic;

namespace GraphDB
{
	public class Graph
	{
		public Dictionary<Category, CategoryVertex> Categories = new Dictionary<Category, CategoryVertex>();

		public Graph ()
		{
		}

		public Graph (Dictionary<Category, CategoryVertex> c)
		{
			Categories = c;
		}

		public void AddCategory(Category cn, CategoryVertex c)
		{
			Categories.Add (cn, c);
		}

		public void AddVertexToCategory(Vertex v, CategoryVertex c, EdgeAttribute a, int weight)
		{
			Edge e = new Edge (c, v, a, weight);

			c.Edges.Add (v, e);
			v.Edges.Add (c, e);
		}

		public void AddEdge(Vertex v, Vertex w, EdgeAttribute a, int weight)
		{
			Edge e = new Edge (w, v, a, weight);

			v.Edges.Add (w, e);
			w.Edges.Add (v, e);
		}

		public void RemoveEdge(Vertex v, Vertex w)
		{
			v.Edges.Remove (w);
			w.Edges.Remove (v);
		}

		public bool TestEdge(Vertex v, Vertex w)
		{
			return v.Edges.ContainsKey (w);
		}

		//returns distance of two neighbors
		public int? CalcDist(Vertex v, Vertex w)
		{
			if (this.TestEdge(v, w))
			{
				return v.Edges [w].Weight;
			} else {
				return null;
			}
		}

		//tests if two vertices are in same category
		public bool TestBridge(Vertex c, Vertex v, Vertex w)
		{
			return c.Edges.ContainsKey(v) && c.Edges.ContainsKey(w);
		}

		//only calculates distance for a single category vertex
		public int? CalcBridgeDist(Vertex c, Vertex v, Vertex w)
		{
			if (this.TestEdge(c, v) && this.TestEdge(c, w))
			{
				return this.CalcDist (c, v) + this.CalcDist (c, w);
			} else {
				return null;
			}
		}
	}
}

