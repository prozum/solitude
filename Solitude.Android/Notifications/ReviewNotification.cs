using System;
using Android.App;
using Android.Widget;
using Android.Graphics;
using Android.Views;
using System.Collections.ObjectModel;

namespace DineWithaDane.Android
{
	public class ReviewNotification : Notification
	{
		ObservableCollection<Notification> notificationList;
		public ReviewNotification (NotificationPosition position, Event evnt, string title, string text, string time, Activity activity, ObservableCollection<Notification> notificationList) : base(position, title, text, time, Color.IndianRed, Color.Red, activity, notificationList)
		{
			this.notificationList = notificationList;

			LinearLayout buttonKeeper = new LinearLayout (activity);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.SetBackgroundColor (Color.Red);

			Button buttonReview = new Button (activity);
			buttonReview.Text = "Review";
			buttonReview.Gravity = GravityFlags.Center;
			buttonReview.SetWidth (displaySize.X / 3);

			Button buttonDismiss = new Button (activity);
			buttonDismiss.Text = "Dismiss";
			buttonDismiss.Gravity = GravityFlags.Center;
			buttonDismiss.SetWidth (displaySize.X / 3);

			buttonReview.Click += (object sender, EventArgs e) => new Review(evnt, activity, notificationList, this);

			buttonDismiss.Click += (object sender, EventArgs e) => 
			{
				Review newReview = new Review (evnt, activity);
			};

			buttonDismiss.Click += (object sender, EventArgs e) => notificationList.Remove(this);

			buttonKeeper.AddView (buttonReview);
			buttonKeeper.AddView (buttonDismiss);

			AddView (buttonKeeper);

			Button filler = new Button (activity);
			filler.Visibility = ViewStates.Invisible;
			AddView (filler);
		}
	}
}

