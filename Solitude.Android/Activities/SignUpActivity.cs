using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ClientCommunication;
using System.Threading;
using Android.Content;

namespace DineWithaDane.Android
{
	[Activity (Label = "Solitude.Android", Theme = "@android:style/Theme.DeviceDefault.NoActionBar")]
	public class SignUpActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			//Setup
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.SignUp);

			//Finds all widgets on the SignUp-layout
			EditText name = FindViewById<EditText> (Resource.Id.editName);
			EditText username = FindViewById<EditText> (Resource.Id.editUsername);
			EditText password = FindViewById<EditText> (Resource.Id.editPassword);
			EditText confirm = FindViewById<EditText> (Resource.Id.editConfirm);
			Button submit = FindViewById<Button> (Resource.Id.buttonSubmit);
			ProgressBar pb = FindViewById<ProgressBar> (Resource.Id.progressSignUp);

			//Hide the progress bar at first
			pb.Visibility = global::Android.Views.ViewStates.Invisible;

			submit.Click += (sender, e) => {
				if(username.Text != "" && password.Text != "" && confirm.Text != "")
				{
					var CIF = new CommunicationInterface();
					pb.Visibility = global::Android.Views.ViewStates.Visible;
					bool succes = false;

					ThreadPool.QueueUserWorkItem( o => {
						succes = CIF.CreateUser(username.Text, password.Text, confirm.Text);

						RunOnUiThread(() => {
							pb.Visibility = global::Android.Views.ViewStates.Invisible;
						});
					});

					if(succes)
					{
						var toLogin = new Intent(this, typeof(MainActivity));
						StartActivity(toLogin);
					}
					else
					{
						var errorDialog = new AlertDialog.Builder(this);
						errorDialog.SetMessage(CIF.LastestError);
						errorDialog.Show();
					}
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

