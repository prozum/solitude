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
		public ReviewNotification (User user, string title, string text, string time, Activity activity, ObservableCollection<Notification> notificationList) : base(user, title, text, time, Color.IndianRed, Color.Red, activity, notificationList)
		{
			var me = this;

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
			buttonLeft.Click += (object sender, EventArgs e) => 
			{
				var dialog = new Dialog(activity);
				dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
				dialog.SetContentView(Resource.Layout.review_layout);

				RatingBar rating = (RatingBar)dialog.FindViewById (Resource.Id.ratingbar);
				dialog.Show();
			};

			buttonRight.Click += (object sender, EventArgs e) => 
			{
				notificationList.Remove(me);
			};

			buttonKeeper.AddView (buttonLeft);
			buttonKeeper.AddView (buttonRight);

			this.AddView (buttonKeeper);

			Button filler = new Button (activity);
			filler.Visibility = ViewStates.Invisible;
			this.AddView (filler);
		}
	}
}

