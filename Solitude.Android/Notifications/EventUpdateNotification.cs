using System;
using Android.Widget;
using Android.App;
using Android.Graphics;
using Android.Views;
using System.Collections.ObjectModel;

namespace DineWithaDane.Android
{
	public class EventUpdateNotification : Notification
	{
		public EventUpdateNotification (NotificationPosition position, string title, string text, string time, Activity activity, ObservableCollection<Notification> notificationList) : base(position, title, text, time, Color.GreenYellow, Color.Green, activity, notificationList)
		{
			var buttonKeeper = new LinearLayout (activity);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.SetBackgroundColor (Color.Green);

			var buttonLeft = new Button (activity);
			buttonLeft.Text = "View";
			buttonLeft.Gravity = GravityFlags.Center;
			buttonLeft.SetWidth (displaySize.X / 3);

			var buttonRight = new Button (activity);
			buttonRight.Text = "Dismiss";
			buttonRight.Gravity = GravityFlags.Center;
			buttonRight.SetWidth (displaySize.X / 3);

			//			buttonLeft.Click

			//			buttonRight.Click

			buttonKeeper.AddView (buttonLeft);
			buttonKeeper.AddView (buttonRight);

			AddView (buttonKeeper);

			var filler = new Button (activity);
			filler.Visibility = ViewStates.Invisible;
			AddView (filler);
		}
	}
}

