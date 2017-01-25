using System;
using System.Collections.Generic;

namespace BBBClasses
{
	public enum NotificationType
	{
		TRADE,		//notify the user about trades
		REVIEW,		//notify the user that their beer has been reviewed
		UPDATE,		//notify the user about an update
		BIRTHDATE   //congratulate user on birthdate
	}

	public class Notification
	{
		public NotificationType Type { get; set; }
		public IEnumerable<string> Data { get; set; }
	}
}

