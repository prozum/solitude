using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Event
	{
		public int Id { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		public int SlotsTotal { get; set; }
		public int SlotsTaken { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Event: Id={0}, Date={1}, Address={2}, Title={3}, Description={4}, UserId={5}, SlotsTaken={6}, SlotsTotal={7}]", Id, Date, Address, Title, Description, UserId, SlotsTaken, SlotsTotal);
		}
	}
}

