using System;

namespace Solitude.Droid
{
	public class Event
	{
		#region Field
		public string Title { get; set; }
		public DateTimeOffset Date { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }
		public int SlotsTotal { get; set; }
		public int SlotsTaken { get; set; }
		public int Id { get; set; }
		#endregion

		#region Constructor
		public Event()
		{
			
		}

		public Event (string title, DateTimeOffset date, string place, string desc, int max, int left)
		{
			Title = title;
			Date = date;
			Address = place;
			Description = desc;
			SlotsTotal = max;
			SlotsTaken = left;
		}

		public Event (string title, DateTimeOffset date, string place, string desc, int max, int left, int ID)
			:this(title, date, place, desc, max, left)
		{
			this.Id = ID;
		}
		#endregion
	}
}

