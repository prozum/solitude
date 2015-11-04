using System;

namespace TileMenu
{
	public class Info
	{
		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public Info(string name, string desc)
		{
			Name = name;
			Description = desc;
		}
	}
}

