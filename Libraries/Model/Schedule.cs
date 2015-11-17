using System;

namespace Model
{
	enum ScheduleType
	{
		EventHeld,
		EventReminder,
		EventSlotsAvailible, 
		Birthdate
	}

	public class Schedule
	{
		public ScheduleType Type { get; set; }
		public DateTimeOffset Date { get; set; }
	}

	public class EventHeld : Schedule
	{
		public int EventId { get; set; }
	}

	public class EventReminder : Schedule
	{
		public int EventId { get; set; }
	}

	public class Birthdate : Schedule
	{
		public string UserId { get; set; }
	}
}

