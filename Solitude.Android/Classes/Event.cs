using System;

namespace DineWithaDane.Android
{
	public class Event
	{
		public string name;
		public int id;
		public string description;

		public Event (string name, int ID, string Description)
		{
			this.name = name;
			id = ID;
			description = Description;
		}

		public override string ToString ()
		{
			return string.Format ("[{0}]: {1}", name, description);
		}
	}
}

