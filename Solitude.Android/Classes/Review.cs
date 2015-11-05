using System;
using System.Collections.Generic;
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

		public Review (User user, Activity currentActivity)
		{
			_user = user;
			_currentActivity = currentActivity;

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
			dialog.Show();

			cancelButton = (Button) dialog.FindViewById <Button> (Resource.Id.cancelReviewButton);
			acceptButton = (Button) dialog.FindViewById <Button> (Resource.Id.postReviewButton);
			input = (EditText)dialog.FindViewById <EditText> (Resource.Id.reviewUserInput);

			cancelButton.Click += (object sender, EventArgs e) => 
			{
				dialog.Dismiss();
			};

			acceptButton.Click += (object sender, EventArgs e) => 
			{
				newReview = input.Text;
			};
		}

		private void sendReview ()
		{
			CommunicationInterface comInterface = new CommunicationInterface ();
			comInterface.PostReview (this);
		}
	}
}

