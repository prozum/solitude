using System;

namespace DineWithaDane.Android
{
	public class Event
	{
		#region Field
		private int _id;

		public string Title { get; set; }
		public DateTime Date { get; set; }
		public string Place { get; set; }
		public string Description { get; set; }
		public int MaxSlots { get; set; }
		public int SlotsLeft { get; set; }
		public int ID {
			get {
				return _id;
			}
		}
		#endregion

		#region Constructor
		public Event (string title, DateTime date, string place, string desc, int max, int left)
		{
			Title = title;
			Date = date;
			Place = place;
			Description = desc;
			MaxSlots = max;
			SlotsLeft = left;
		}
		#endregion

		#region Public Methods
		#endregion

		#region Privat Methods

		#endregion
	}
}

