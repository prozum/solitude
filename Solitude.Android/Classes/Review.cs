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
		Event _event;
		Activity _currentActivity;
		ObservableCollection<Notification> _notificationList;
		ReviewNotification _notification;
		RatingBar rating;
		string newReview;

		public string ReviewText 
		{
			get {
				return newReview;
			}
		}

		public int Rating
		{
			get {
				return (int) rating.Rating;	
			}
		}

		public Event Event
		{
			get
			{
				return _event;
			}
		
		}

		#endregion
		public Review (Event e, Activity currentActivity){
			_event = e;
			_currentActivity = currentActivity;
		}

		public Review (Event e, Activity currentActivity, ObservableCollection<Notification> notificationList, ReviewNotification notification)
			: this (e, currentActivity)
		{
			_notificationList = notificationList;
			_notification = notification;

			setupReviewDialog ();
		}

		private void setupReviewDialog ()
		{
			var dialog = new Dialog(_currentActivity);
			Button cancelButton, acceptButton;
			EditText input;
			TextView reviewTitle;

			dialog.RequestWindowFeature((int) WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.review_layout);

			reviewTitle = dialog.FindViewById<TextView>(Resource.Id.reviewTitle);
			rating = (RatingBar)dialog.FindViewById (Resource.Id.ratingbar);
			input = (EditText)dialog.FindViewById <EditText> (Resource.Id.reviewUserInput);

			reviewTitle.Text = "Review of " + _event.Title;

			acceptButton = (Button) dialog.FindViewById <Button> (Resource.Id.postReviewButton);
			cancelButton = (Button) dialog.FindViewById <Button> (Resource.Id.cancelReviewButton);

			dialog.Show();

			cancelButton.Click += (object sender, EventArgs e) => 
			{
				dialog.Dismiss();
			};

			acceptButton.Click += (object sender, EventArgs e) => 
			{
				newReview = input.Text;
				sendReview();
				dialog.Dismiss();
				_notificationList.Remove(_notification);
			};
		}

		/// <summary>
		/// Sends the review to the server.
		/// </summary>
		private void sendReview ()
		{
			MainActivity.CIF.PostReview(this);
		}
	}
}

