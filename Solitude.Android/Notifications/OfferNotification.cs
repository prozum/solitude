using System;
using Android.App;
using Android.Graphics;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;

namespace DineWithaDane.Android
{
	public class OfferNotification : Notification
	{
		public OfferNotification (string title, string text, string time, Activity activity, ObservableCollection<Notification> notificationList) : base(Direction.Guest, title, text, time, Color.OrangeRed, Color.Orange, activity, notificationList)
		{
			LinearLayout buttonKeeper = new LinearLayout (activity);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.SetBackgroundColor (Color.Orange);

			Button buttonLeft = new Button (activity);
			buttonLeft.Text = "View";
			buttonLeft.Gravity = GravityFlags.Center;
			buttonLeft.SetWidth (displaySize.X / 3);

			//			buttonLeft.Click += 

			buttonKeeper.AddView (buttonLeft);
			this.AddView (buttonKeeper);

			Button filler = new Button (activity);
			filler.Visibility = ViewStates.Invisible;
			this.AddView (filler);
		}
	}
}

