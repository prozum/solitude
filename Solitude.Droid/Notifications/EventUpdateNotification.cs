using System;
using Android.Widget;
using Android.App;
using Android.Graphics;
using Android.Views;
using System.Collections.ObjectModel;

namespace Solitude.Droid
{
	public class EventUpdateNotification : Notification
	{
		public EventUpdateNotification (string eventTitle, string oldValue, string newValue, Activity activity, ObservableCollection<Notification> notificationList) 
			: base(eventTitle, "Information was changed in above event:", string.Format("{0} was changed to {1}", oldValue, newValue), Color.GreenYellow, Color.Green, activity, notificationList)
		{
			LeftButton.Text = "Cancel";

			RightButton.Text = "View";
		}
	}
}

