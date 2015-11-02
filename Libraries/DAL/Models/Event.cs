using System;

namespace DAL
{
	public class Event
	{
		public string Date { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public int eid { get; set; }
		public string uid { get; set; }

		public Event (string date, string address, string description, string uid, int eid)
		{
			Date = date;
			Address = address;
			Description = description;
			this.uid = uid;
			this.eid = eid;
		}
	}
}

