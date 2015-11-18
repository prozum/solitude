using System;

namespace Model
{
	public enum NotificationType
	{
		OFFER,		//notify the user about offers
		REVIEW,		//notify the user that they have a pending review
		UPDATE,		//notify the user about an event update
		REMINDER,	//remind the user about upcoming events
		BIRTHDATE   //congratulate user on birthdate
	}

	public class Notification
	{
		public NotificationType Type { get; set; }
		public string Message { get; set; }
		public string UserId { get; set; }
		public int EventId { get; set; }

	}
}

