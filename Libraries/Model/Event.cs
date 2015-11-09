using System;

namespace Model
{
	public class Event
	{
		public int ID { get; set; }
		public string Date { get; set; }
		public string Address { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string UserID { get; set; }
		public int SlotsTaken { get; set; }
		public int SlotsTotal { get; set; }
	}
}

