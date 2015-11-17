using System;

namespace Model
{
	public enum TaskType
	{
		EventHeld,
		EventReminder,
		EventSlotsAvailable, 
		Birthdate
	}

	public class TaskData
	{
		public TaskType Type { get; set; }
		public DateTimeOffset DateStart { get; set; }
		public DateTimeOffset DateEnd { get; set; }
		public bool RunAfterDateEnd { get; set; }
		public int EventId { get; set; }
		public string UserId { get; set; }
	}

//	public class EventHeld : TaskData
//	{
//		public int EventId { get; set; }
//	}
//
//	public class EventReminder : TaskData
//	{
//		public int EventId { get; set; }
//	}
//
//	public class Birthdate : TaskData
//	{
//		public string UserId { get; set; }
//	}
}

