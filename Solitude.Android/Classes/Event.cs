using System;

namespace DineWithaDane.Android
{
	public class Event
	{
		#region Field
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string Place { get; set; }
		public string Description { get; set; }
		public int MaxSlots { get; set; }
		public int SlotsLeft { get; set; }
		public int ID { get; set; }
		#endregion

		#region Constructor
		public Event()
		{
			
		}

		public Event (string title, DateTime date, string place, string desc, int max, int left)
		{
			Title = title;
			Date = date;
			Place = place;
			Description = desc;
			MaxSlots = max;
			SlotsLeft = left;
		}

		public Event (string title, DateTime date, string place, string desc, int max, int left, int ID)
			:this(title, date, place, desc, max, left)
		{
			this.ID = ID;
		}
		#endregion
	}
}

