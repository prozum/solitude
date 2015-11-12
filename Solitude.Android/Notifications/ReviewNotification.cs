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
		public ReviewNotification (Event evnt, Activity activity, ObservableCollection<Notification> notificationList) 
			: base(evnt.Title, evnt.Description, evnt.Date.ToString(), activity, notificationList)
		{
			//Initialize the two buttons
			RightButton.Text = "Review";

			LeftButton.Text = "Dismiss";

			//Add functionality to buttons
			this.Click /*buttonReview.Click*/ += (object sender, EventArgs e) => new Review(evnt, activity, notificationList, this);
			LeftButton.Click += (object sender, EventArgs e) => 
				{
					Review newReview = new Review (evnt, activity);
					notificationList.Remove(this);
				};
		}
	}
}

