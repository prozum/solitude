using System;
using System.Collections.Generic;

namespace GraphDB
{
	public class CategoryVertex : Vertex
	{
		public readonly Category CategoryName; 

		public CategoryVertex (Category c) : base()
		{
			CategoryName = c;
		}
	}
}