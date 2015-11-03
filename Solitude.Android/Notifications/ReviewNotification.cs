using System;
using Android.App;
using Android.Widget;
using Android.Graphics;
using Android.Views;

namespace DineWithaDane.Android
{
	public class ReviewNotification : Notification
	{
		public ReviewNotification (User user, string title, string text, string time, Activity activity) : base(user, title, text, time, Color.IndianRed, Color.Red, activity)
		{
			LinearLayout buttonKeeper = new LinearLayout (activity);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.SetBackgroundColor (Color.Red);

			Button buttonLeft = new Button (activity);
			buttonLeft.Text = "Review";
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

