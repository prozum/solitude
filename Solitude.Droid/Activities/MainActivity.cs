using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.Threading;
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	[Activity(Label = "Let's hangout", MainLauncher = true)]
	public class MainActivity : Activity
	{
		public static ClientCommunication.CommunicationInterface CIF{ get; private set; }

		public static string[][] InfoNames { get; private set; }

		public static string[] InfoTitles { get; private set; }

		public static Tuple<int, Type>[] DrawerActivities { get; private set; }

		public static string[] DrawerNames { get; private set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			StartService(new Intent(this, typeof(BackgroundService)));

			CIF = new ClientCommunication.CommunicationInterface();

			if (InfoNames == null)
				InfoNames = new string[][]
				{
					Resources.GetStringArray(Resource.Array.languages),
					Resources.GetStringArray(Resource.Array.interests),
					Resources.GetStringArray(Resource.Array.foodhabits)
				};

			if (InfoTitles == null)
				InfoTitles = Resources.GetStringArray(Resource.Array.info_titles);

			if (DrawerActivities == null)
				DrawerActivities = new Tuple<int, Type>[]
				{
					new Tuple<int, Type>(Resource.Drawable.Profile_Icon, typeof(ProfileActivity)),
					new Tuple<int, Type>(Resource.Drawable.Invitation_Icon, typeof(OfferActivity)),
					new Tuple<int, Type>(Resource.Drawable.Events_Icon, typeof(EventActivity)),
					new Tuple<int, Type>(Resource.Drawable.Host_Icon, typeof(HostActivity)),
					new Tuple<int, Type>(Resource.Drawable.Settings_Icon, typeof(SettingsActivitiy)),
					new Tuple<int, Type>(Resource.Drawable.Logout_Icon, typeof(MainActivity))
				};

			if (DrawerNames == null)
				DrawerNames = Resources.GetStringArray(Resource.Array.drawer_items);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			Button loginButton = FindViewById<Button>(Resource.Id.buttonLogin);
			Button signUp = FindViewById<Button>(Resource.Id.buttonSignUp);
			var layout = FindViewById<LinearLayout>(Resource.Id.layoutMain);

			//Adds a delegates to the buttons
			loginButton.Click += loginButtonClicked;
			signUp.Click += (sender, e) =>
			{
				var signUpScreenIntent = new Intent(this, typeof(SignUpActivity));
				StartActivity(signUpScreenIntent);
			};

			#if DEBUG
			var skip = new Button(this);
			skip.Text = "Skip";
			skip.Click += (sender, e) =>
			{
				var toProfile = new Intent(this, typeof(ProfileActivity));
				StartActivity(toProfile);
			};
			layout.AddView(skip);
			#endif
		}

		/// <summary>
		/// Login using credidentials specified in the text fields.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void loginButtonClicked(object sender, EventArgs e)
		{
			//Finds the views from the layout
			var loginButton = FindViewById<Button>(Resource.Id.buttonLogin);
			var username = FindViewById<EditText>(Resource.Id.editUsername);
			var password = FindViewById<EditText>(Resource.Id.editPassword);

			loginButton.Clickable = false;

			#if DEBUG
			if (String.IsNullOrEmpty(username.Text) && String.IsNullOrEmpty(password.Text))
			{
				username.Text = "test";
				password.Text = "Test123%";
			}
			#endif

			//Tries to login if strings are present in the fields
			if (username.Text != "" || password.Text != "")
			{
				bool loggedIn = false;

				//Adds a spinner to indicate loading
				ProgressBar pb = new ProgressBar(this);
				LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.layoutMain);
				layout.AddView(pb);

				//Does server communication on separate thread to avoid UI-freeze
				ThreadPool.QueueUserWorkItem(o =>
					{
						if (Looper.MyLooper() == null)
							Looper.Prepare();

						loggedIn = CIF.Login(username.Text, password.Text);

						//Moves on to next activity, if login is succesful
						if (loggedIn)
						{
							CIF.GetInformation();
							Intent toProfile = new Intent(this, typeof(ProfileActivity));
							StartActivity(toProfile);
						}
						//Shows an error-messaage, if login was not succesful
						else
						{
							var loginFailedDialog = new AlertDialog.Builder(this);
							loginFailedDialog.SetMessage(CIF.LatestError);
							loginFailedDialog.SetNegativeButton(Resource.String.ok, (s, earg) =>
								{
								});
							RunOnUiThread(() =>
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
				noTextAlert.SetMessage(Resources.GetString(Resource.String.message_empty_username_password));
				noTextAlert.SetNegativeButton(Resource.String.ok, (s, earg) =>
					{
					});
				noTextAlert.Show();

				loginButton.Clickable = true;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			StopService(new Intent(this, typeof(BackgroundService)));
		}
	}
}