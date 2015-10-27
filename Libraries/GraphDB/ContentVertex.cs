using System;

namespace GraphDB
{
	public class ContentVertex : Vertex
	{
		readonly Object _content;

		public ContentVertex (Object content) : base()
		{
			_content = content;
		}

		public T GetContent<T> ()
		{
			return (T)_content;
		}
	}
}

