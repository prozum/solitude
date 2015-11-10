using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DineWithaDane.Android;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.Views;
using ClientCommunication;

namespace DineWithaDane.Android
{
	public class Review
	{
		#region properties
		Activity currentActivity;
		ObservableCollection<Notification> notificationList;
		ReviewNotification notification;
		RatingBar rating;

		public string ReviewText { get; private set; }
		public Event Event { get; private set; }
		public int Rating { get { return (int) rating.Rating; } }


		#endregion

		public Review (Event e, Activity currentActivity)
		{
			Event = e;
			currentActivity = currentActivity;
		}

		public Review (Event e, Activity currentActivity, 
					   ObservableCollection<Notification> notificationList, 
					   ReviewNotification notification)
			: this (e, currentActivity)
		{
			notificationList = notificationList;
			notification = notification;

			setupReviewDialog ();
		}

		private void setupReviewDialog ()
		{
			var dialog = new Dialog(currentActivity);
			Button cancelButton, acceptButton;
			EditText input;
			TextView reviewTitle;

			dialog.RequestWindowFeature((int) WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.review_layout);

			reviewTitle = dialog.FindViewById<TextView>(Resource.Id.reviewTitle);
			rating = (RatingBar)dialog.FindViewById (Resource.Id.ratingbar);
			input = (EditText)dialog.FindViewById <EditText> (Resource.Id.reviewUserInput);

			reviewTitle.Text = "Review of " + Event.Title;

			acceptButton = (Button) dialog.FindViewById <Button> (Resource.Id.postReviewButton);
			cancelButton = (Button) dialog.FindViewById <Button> (Resource.Id.cancelReviewButton);

			dialog.Show();

			cancelButton.Click += (object sender, EventArgs e) => dialog.Dismiss();
			acceptButton.Click += (object sender, EventArgs e) => 
				{
					ReviewText = input.Text;
					SendReview();
					dialog.Dismiss();
					notificationList.Remove(notification);
				};
		}

		/// <summary>
		/// Sends the review to the server.
		/// </summary>
		private void SendReview ()
		{
			MainActivity.CIF.PostReview(this);
		}
	}
}

