using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ClientCommunication;
using System.Threading;
using Android.Content;
using Android.Views;

namespace DineWithaDane.Android
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

			@continue.Click += (sender, e) =>
			{
				var pb = new ProgressBar(this);
				layout.AddView(pb);

				if (username.Text != "" && password.Text != "" && confirm.Text != "" && name.Text != "" && address.Text != "")
				{
					@continue.Clickable = false;

					ThreadPool.QueueUserWorkItem(o =>
						{
							RunOnUiThread(() => pb.Visibility = ViewStates.Visible);

							if (MainActivity.CIF.CreateUser(name.Text, address.Text, birthday.DateTime, username.Text, password.Text, confirm.Text) &&
							    MainActivity.CIF.Login(username.Text, password.Text))
							{
								var toProfile = new Intent(this, typeof(ProfileActivity));
								StartActivity(toProfile);
							}
							else
							{
								var errorDialog = new AlertDialog.Builder(this);
								errorDialog.SetMessage(MainActivity.CIF.LatestError);
								RunOnUiThread(() => errorDialog.Show());
							}

							//Removes the spinner again
							RunOnUiThread(() =>
								{
									layout.RemoveView(pb);
									pb.Dispose();
								});

							@continue.Clickable = true;
						});

					/*
						@continue.Clickable = false;
						var CIF = new CommunicationInterface();
						pb.Visibility = global::Android.Views.ViewStates.Visible;
						bool success = false;

						ThreadPool.QueueUserWorkItem( o => {
							success = CIF.CreateUser(username.Text, password.Text, confirm.Text);

							RunOnUiThread(() => {
								pb.Visibility = global::Android.Views.ViewStates.Invisible;
							});

							if(success)
							{
								var toLogin = new Intent(this, typeof(MainActivity));
								StartActivity(toLogin);
							}
							else
							{
								var errorDialog = new AlertDialog.Builder(this);
								errorDialog.SetMessage(CIF.LatestError);
								RunOnUiThread(() => {
									errorDialog.Show();
								});

								@continue.Clickable = true;
							}
						});
						*/
				}
				else
				{
					var errorDialog = new AlertDialog.Builder(this);
					errorDialog.SetMessage("Missing some information, please make sure all fields are filled correctly");
					errorDialog.Show();
				}
			};
		}
	}
}

