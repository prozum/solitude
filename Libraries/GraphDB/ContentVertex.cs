using System;

namespace libgraph
{
	public class ContentVertex : Vertex
	{
		public readonly Object Content;

		public ContentVertex (Object content) : base()
		{
			Content = content;
		}
	}
}

