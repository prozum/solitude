using System;

namespace Dal
{
	public class Event
	{
		public string Date { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public int eid { get; set; }
		public string uid { get; set; }
		public int SlotsLeft { get; set; }

		public Event (string date, string address, string description, int slots, string uid, int eid)
		{
			Date = date;
			Address = address;
			Description = description;
			this.uid = uid;
			this.eid = eid;
			this.SlotsLeft = slots;
		}
	}
}

