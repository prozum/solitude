using System;

namespace DAL
{
	public class Event
	{
		public string Date { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }

		public Event() {}

		public Event(Event e)
		{
			Date = e.Date;
			Address = e.Address;
			Description = e.Description;
		}
	}
}

