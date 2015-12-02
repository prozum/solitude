using System;

namespace Solitude.Droid
{
	public class Event
	{
		#region Field
		public string Title { get; set; }
		public DateTimeOffset Date { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public int SlotsTotal { get; set; }
		public int SlotsTaken { get; set; }
		public string Id { get; set; }
		#endregion
	}
}

