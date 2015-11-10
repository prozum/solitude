using System;

namespace Model
{
	public class Event
	{
		public int Id { get; set; }
		public string Date { get; set; }
		public string Address { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string UserId { get; set; }
		public int SlotsTaken { get; set; }
		public int SlotsTotal { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Event: Id={0}, Date={1}, Address={2}, Title={3}, Description={4}, UserId={5}, SlotsTaken={6}, SlotsTotal={7}]", Id, Date, Address, Title, Description, UserId, SlotsTaken, SlotsTotal);
		}
	}
}

