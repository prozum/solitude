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
			};
		}

		private void sendReview ()
		{
			CommunicationInterface comInterface = new CommunicationInterface ();
			comInterface.PostReview (this);
		}
	}
}

