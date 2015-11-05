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
		public EventUpdateNotification (Direction user, string title, string text, string time, Activity activity, ObservableCollection<Notification> notificationList) : base(user, title, text, time, Color.GreenYellow, Color.Green, activity, notificationList)
		{
			LinearLayout buttonKeeper = new LinearLayout (activity);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.SetBackgroundColor (Color.Green);

			Button buttonLeft = new Button (activity);
			buttonLeft.Text = "View";
			buttonLeft.Gravity = GravityFlags.Center;
			buttonLeft.SetWidth (displaySize.X / 3);

			Button buttonRight = new Button (activity);
			buttonRight.Text = "Dismiss";
			buttonRight.Gravity = GravityFlags.Center;
			buttonRight.SetWidth (displaySize.X / 3);

			//			buttonLeft.Click

			//			buttonRight.Click

			buttonKeeper.AddView (buttonLeft);
			buttonKeeper.AddView (buttonRight);

			this.AddView (buttonKeeper);

			Button filler = new Button (activity);
			filler.Visibility = ViewStates.Invisible;
			this.AddView (filler);
		}
	}
}

