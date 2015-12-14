using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace Solitude.Droid
{
    /// <summary>
    /// The MainActivity acts as the entry point for the application, and is also the login screen.
    /// </summary>
	[Activity(Label = "FriendLine", MainLauncher = true)]
	public class MainActivity : Activity
	{
        /// <summary>
        /// The accesspoint for the communication interface. All communication with the server should go through this instance of the interface.
        /// </summary>
		public static ClientCommunication.CommunicationInterface CIF{ get; private set; }

        /// <summary>
        /// A tuple containing all the activities located in the drawer.
        /// </summary>
		public static Tuple<int, Type>[] DrawerActivities { get; private set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			//Start up necesarry classes and services
			CIF = new ClientCommunication.CommunicationInterface();

			//Sets up icons and actions in the drawer
			if (DrawerActivities == null)
				DrawerActivities = new Tuple<int, Type>[]
				{
					new Tuple<int, Type>(Resource.Drawable.Profile_Icon, typeof(ProfileActivity)),
					new Tuple<int, Type>(Resource.Drawable.Events_Icon, typeof(EventActivity)),
					new Tuple<int, Type>(Resource.Drawable.Logout_Icon, typeof(MainActivity))
				};

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			var loginButton = FindViewById<Button>(Resource.Id.buttonLogin);
			var signUp = FindViewById<TextView>(Resource.Id.textSignUp);
			var layout = FindViewById<LinearLayout>(Resource.Id.loginLinear);

			//Adds delegates to and sets layout of the buttons
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
			var loginButton = FindViewById<Button>(Resource.Id.buttonLogin);
			var username = FindViewById<EditText>(Resource.Id.editUsername);
			var password = FindViewById<EditText>(Resource.Id.editPassword);

            // Prevents the user from clicking the login button after being clicked, while communicating with the server.
			loginButton.Clickable = false;

            // Debug only, allows for easy login on the server if no input is given.
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
				LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.loginLinear);
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
							var loginFailedDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
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
				var noTextAlert = new Android.Support.V7.App.AlertDialog.Builder(this);
				noTextAlert.SetMessage(Resources.GetString(Resource.String.message_empty_username_password));
				noTextAlert.SetNegativeButton(Resource.String.ok, (s, earg) =>
					{
					});
				noTextAlert.Show();

				loginButton.Clickable = true;
			}
		}
	}
}