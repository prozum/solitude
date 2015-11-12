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
	[Activity (Label = "Let's hangout", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public static ClientCommunication.CommunicationInterface CIF{ get; private set;}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			StartService (new Intent(this, typeof(BackgroundService)));

			CIF = new ClientCommunication.CommunicationInterface ();

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			Button loginButton = FindViewById<Button> (Resource.Id.buttonLogin);
			Button signUp = FindViewById<Button> (Resource.Id.buttonSignUp);

			//Adds a delegates to the buttons
			loginButton.Click += loginButtonClicked;
			signUp.Click += (sender, e) => 
				{
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

			loginButton.Clickable = false;

			#if DEBUG
			if (String.IsNullOrEmpty(username.Text) && String.IsNullOrEmpty(password.Text))
			{
				username.Text = "test";
				password.Text = "Test123%";
			}
			#endif

			//Tries to login if strings are present in the fields
			if(username.Text != "" || password.Text != "")
			{
				bool loggedIn = false;

				//Adds a spinner to indicate loading
				ProgressBar pb = new ProgressBar(this);
				LinearLayout layout = FindViewById<LinearLayout> (Resource.Id.layoutMain);
				layout.AddView(pb);

				//Does server communication on separate thread to avoid UI-freeze
				ThreadPool.QueueUserWorkItem(o => 
					{
						loggedIn = CIF.Login(username.Text, password.Text);

						//Moves on to next activity, if login is succesful
						if(loggedIn)
						{
							Intent toProfile = new Intent(this, typeof(ProfileActivity));
							StartActivity(toProfile);
						}
						//Shows an error-messaage, if login was not succesful
						else
						{
							var loginFailedDialog = new AlertDialog.Builder(this);
							loginFailedDialog.SetMessage(CIF.LatestError);
							RunOnUiThread( () => 
							{
								loginFailedDialog.Show();
							});
						}

						//Removes the spinner again
						RunOnUiThread(() =>
							{
								layout.RemoveView(pb);
								pb.Dispose();
							});

						loginButton.Clickable = true;
					});
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


