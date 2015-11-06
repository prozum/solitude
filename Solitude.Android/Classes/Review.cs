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
		User _user;
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
				return rating.NumStars;	
			}
		}

		public Review (User user, Activity currentActivity, ObservableCollection<Notification> notificationList, ReviewNotification notification)
		{
			_user = user;
			_currentActivity = currentActivity;
			_notificationList = notificationList;
			_notification = notification;

			setupReviewDialog ();
		}

		private void setupReviewDialog ()
		{
			var dialog = new Dialog(_currentActivity);
			Button cancelButton, acceptButton;
			EditText input;

			dialog.RequestWindowFeature((int) WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.review_layout);

			rating = (RatingBar)dialog.FindViewById (Resource.Id.ratingbar);
			input = (EditText)dialog.FindViewById <EditText> (Resource.Id.reviewUserInput);

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
				dialog.Dismiss();
				_notificationList.Remove(_notification);
			};
		}

		private void sendReview ()
		{
			CommunicationInterface comInterface = new CommunicationInterface ();
			comInterface.PostReview (this);
		}
	}
}

