using System;
using Android.App;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using System.Collections.ObjectModel;

namespace DineWithaDane.Droid
{
	public class EventReminderNotification : Notification
	{
		public EventReminderNotification (Event e, Activity activity, ObservableCollection<Notification> notificationList) 
			: base(e.Title, e.Description, e.Date.ToString(), activity, notificationList)
		{
			//Initialize two buttons
			RightButton.Text = "View";

			LeftButton.Text = "Cancel";

			PlaceNotificationImage(Resource.Drawable.Event_Inv);
		}
	}
}

