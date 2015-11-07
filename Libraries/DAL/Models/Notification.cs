using System;

namespace Dal
{
	public class Notification
	{
		public enum NotificationType
		{
			OFFER,		//notify the user about offers
			REVIEW,		//notify the user that they have a pending review
			UPDATE,		//notify the user about an event update
			REMINDER	//remind the user about upcoming events
		}

		public NotificationType @Type;

		public Notification (NotificationType type)
		{
			Type = type;
		}
	}
}

