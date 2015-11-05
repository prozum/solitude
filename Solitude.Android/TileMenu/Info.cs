using System;

namespace DineWithaDane.Android
{
	public class InfoTest
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

		public InfoTest(string name, string desc)
		{
			Name = name;
			Description = desc;
		}
	}
}

