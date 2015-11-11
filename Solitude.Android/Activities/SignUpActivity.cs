using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ClientCommunication;
using System.Threading;
using Android.Content;

namespace DineWithaDane.Android
{
	[Activity (Label = "Solitude.Android")]
	public class SignUpActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			//Setup
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.SignUp);

			//Finds all widgets on the SignUp-layout
			EditText username = FindViewById<EditText> (Resource.Id.editUsername);
			EditText password = FindViewById<EditText> (Resource.Id.editPassword);
			EditText confirm = FindViewById<EditText> (Resource.Id.editConfirm);
			Button @continue = FindViewById<Button> (Resource.Id.buttonContinue);
			ProgressBar pb = FindViewById<ProgressBar> (Resource.Id.progressSignUp);

			//Hide the progress bar at first
			pb.Visibility = global::Android.Views.ViewStates.Invisible;

			@continue.Click += (sender, e) => 
				{
					if(username.Text != "" && password.Text != "" && confirm.Text != "")
					{
						@continue.Clickable = false;

						ThreadPool.QueueUserWorkItem( o => {
							RunOnUiThread(() => {
								pb.Visibility = global::Android.Views.ViewStates.Invisible;
							});

							if (MainActivity.CIF.ConfirmUser(username.Text, password.Text, confirm.Text)) 
							{
								var toInfo = new Intent(this, typeof(SignUpInfoActivity));
								toInfo.PutExtra("username", username.Text);
								toInfo.PutExtra("password", password.Text);
								StartActivity(toInfo);
							} 
							else 
							{
								var errorDialog = new AlertDialog.Builder(this);
								errorDialog.SetMessage(MainActivity.CIF.LatestError);
								RunOnUiThread(() => {
									errorDialog.Show();
								});
								@continue.Clickable = true;
							}
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

