using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Threading;

namespace DineWithaDane.Android
{
	[Activity (Label = "Solitude.Android", MainLauncher = true, Theme = "@android:style/Theme.DeviceDefault.NoActionBar")]
	public class MainActivity : Activity
	{
		private ClientCommunication.CommunicationInterface CIF;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			StartService (new Intent(this, typeof(BackgroundService)));

			CIF = new ClientCommunication.CommunicationInterface ();
			List<Offer> offers;

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			ProgressBar loginProgress = FindViewById<ProgressBar> (Resource.Id.progressLogin);
			Button loginButton = FindViewById<Button> (Resource.Id.buttonLogin);
			Button signUp = FindViewById<Button> (Resource.Id.buttonSignUp);

			loginProgress.Visibility = ViewStates.Invisible;

			//Adds a delegates to the buttons
			loginButton.Click += loginButtonClicked;
			signUp.Click += (sender, e) => {
				var signUpScreenIntent = new Intent(this, typeof(SignUpActivity));
				StartActivity(signUpScreenIntent);
			};
		}

		/// <summary>
		/// Login using credidentials specified in the text fields.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void loginButtonClicked(object sender, EventArgs e)
		{
			//Finds the views from the layout
			Button loginButton = FindViewById<Button> (Resource.Id.buttonLogin);
			EditText username = FindViewById<EditText> (Resource.Id.editUsername);
			EditText password = FindViewById<EditText> (Resource.Id.editPassword);
			ProgressBar loginProgress = FindViewById<ProgressBar> (Resource.Id.progressLogin);

			//Tries to login if strings are present in the fields
			if(username.Text != "" || password.Text != "")
			{
				loginProgress.Visibility = ViewStates.Visible;
				bool loggedIn = false;

				ThreadPool.QueueUserWorkItem(o => {
					loggedIn = CIF.Login(username.Text, password.Text);

					RunOnUiThread( () => {
						loginProgress.Visibility = ViewStates.Invisible;
					});
				});
				//Moves on to next activity, if login is succesful
				if(loggedIn)
				{
					Intent toNotificationScreen = new Intent(this, typeof(NotificationActivity));
				}
			}
			//Displays an errormessage, if no username or password is entered
			else
			{
				var noTextAlert = new AlertDialog.Builder(this);
				noTextAlert.SetMessage("No text entered in either username or password field");
				noTextAlert.Show();
			}
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			StopService (new Intent (this, typeof(BackgroundService)));
		}
	}
}


