using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ClientCommunication;
using System.Threading;
using Android.Content;
using Android.Views;

namespace Solitude.Droid
{
	[Activity(Label = "Solitude.Android")]
	public class SignUpActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			//Setup
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SignUp);

			//Finds all widgets on the SignUp-layout
			var name = FindViewById<EditText>(Resource.Id.editSignUpName);
			var birthday = FindViewById<DatePicker>(Resource.Id.signupBirthday);
			var address = FindViewById<EditText>(Resource.Id.editAddress);
			var username = FindViewById<EditText>(Resource.Id.editUsername);
			var password = FindViewById<EditText>(Resource.Id.editPassword);
			var confirm = FindViewById<EditText>(Resource.Id.editConfirm);
			var @continue = FindViewById<Button>(Resource.Id.buttonContinue);
			var layout = FindViewById<LinearLayout>(Resource.Id.layout);

			birthday.MaxDate = new Java.Util.Date().Time;

			@continue.Click += (sender, e) =>
			{

				if (username.Text != "" && password.Text != "" && confirm.Text != "" && name.Text != "" && address.Text != "")
				{
					var pb = new AlertDialog.Builder(this).Create();
					pb.SetView(new ProgressBar(this));
					pb.SetCancelable(false);
					RunOnUiThread(() => pb.Show());

					@continue.Clickable = false;

					ThreadPool.QueueUserWorkItem(o =>
						{
							if (MainActivity.CIF.CreateUser(name.Text, address.Text, new DateTimeOffset(birthday.DateTime, new TimeSpan(0)), username.Text, password.Text, confirm.Text) &&
						    MainActivity.CIF.Login(username.Text, password.Text))
							{
								var dialog = new AlertDialog.Builder(this);
								dialog.SetTitle(Resources.GetString(Resource.String.sign_up_success));
								dialog.SetMessage(Resources.GetString(Resource.String.sign_up_set_info));
								dialog.SetCancelable(false);
								dialog.SetNegativeButton(Resources.GetString(Resource.String.no), delegate
									{ 
										var toProfile = new Intent(this, typeof(ProfileActivity));
										StartActivity(toProfile);
									});
								dialog.SetNeutralButton(Resources.GetString(Resource.String.yes), delegate
									{
										var toSettings = new Intent(this, typeof(SettingsActivitiy));
										toSettings.PutExtra("index", 4);
										StartActivity(toSettings);
									});
								RunOnUiThread(() => dialog.Show());

							}
							else
							{
								var errorDialog = new AlertDialog.Builder(this);
								errorDialog.SetMessage(MainActivity.CIF.LatestError);
								errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => {});
								RunOnUiThread(() => errorDialog.Show());
							}

							//Removes the spinner again
							RunOnUiThread(() => pb.Dismiss());

							@continue.Clickable = true;
						});
				}
				else
				{
					var errorDialog = new AlertDialog.Builder(this);
					errorDialog.SetMessage(Resources.GetString(Resource.String.sign_up_missing_info));
					errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => {});
					errorDialog.Show();
				}
			};
		}
	}
}

