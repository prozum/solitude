using System;
using System.Collections.Generic;

namespace GraphDB
{
	public class Graph
	{
		List<CategoryVertex> categories = new List<CategoryVertex>();

		public Graph ()
		{
			
		}

		public void AddCategory(CategoryVertex c)
		{
			categories.Add (c);
		}

		public void AddVertexToCategory(Vertex v, CategoryVertex c, EdgeAttribute a, int weight)
		{
			v.Edges.Add (new Edge(v, c, a, weight));
			c.Edges.Add (new Edge(c, v, a, weight));
		}

		public void RemoveVertexFromCategory(Vertex v, CategoryVertex c)
		{
			v.Edges.RemoveAll (e => e.Node2 == c);
			c.Edges.RemoveAll (e => e.Node2 == v);
		}

		public CategoryVertex SearchCategory(Category c)
		{
			return categories.Find (cc => c == cc.CategoryName);
		}

		//find a category from a given CategoryVertex cv
		public CategoryVertex SearchCategory(CategoryVertex cv, Category c)
		{
			List<Edge> ce = cv.Edges.FindAll (e => e.Node2.GetType() == typeof(CategoryVertex));
			return (CategoryVertex)ce.Find (e => ((CategoryVertex)(e.Node2)).CategoryName == c).Node2;
			//return ce.Find (v => ((CategoryVertex)(v.Node2)).CategoryName == c);
		}

		public void AddEdge(ContentVertex v, ContentVertex w, EdgeAttribute a, int weight)
		{
			v.Edges.Add (new Edge(v, w, a, weight));
			w.Edges.Add (new Edge(w, v, a, weight));
		}

		public void RemoveEdge(Vertex v, Vertex w)
		{
			v.Edges.RemoveAll (e => e.Node2 == w);
			w.Edges.RemoveAll (e => e.Node2 == v);
		}

		public bool TestEdge(Vertex v, Vertex w)
		{
			if (v.Edges.Find (e => e.Node2 == w) != null)
			{
				return true;
			} else {
				return false;
			}
		}
	}
}

