using System;
using Android.App;
using Android.OS;
using Android.Widget;
using ClientCommunication;
using System.Threading;
using Android.Content;
using Android.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android;

namespace Solitude.Droid
{
	[Activity(Label = "Solitude.Android")]
	public class SignUpActivity : FragmentActivity
	{

		public static CustomViewPager _viewPager;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			//Setup
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SignUp);

			//Finds all widgets on the SignUp-layout
			View fragView1 = View.Inflate(this, Resource.Layout.signupFragLayout1, null);
			View fragView2 = View.Inflate(this, Resource.Layout.signupFragLayout2, null);
			View fragView3 = View.Inflate(this, Resource.Layout.signupFragLayout3, null);
			View fragView4 = View.Inflate(this, Resource.Layout.signupFragLayout4, null);
			View fragView5 = View.Inflate(this, Resource.Layout.signupFragLayout5, null);


			var name = (EditText) fragView1.FindViewById<EditText>(Resource.Id.editSignUpName);
			var birthday = (DatePicker) fragView2.FindViewById<DatePicker>(Resource.Id.signupBirthday);
			var address = (EditText) fragView1.FindViewById<EditText>(Resource.Id.editAddress);
			var username = (EditText) fragView3.FindViewById<EditText>(Resource.Id.editUsername);
			var password = (EditText) fragView3.FindViewById<EditText>(Resource.Id.editPassword);
			var confirm = (EditText) fragView3.FindViewById<EditText>(Resource.Id.editConfirm);
			//var layout = FindViewById<LinearLayout>(Resource.Id.layout);

			birthday.MaxDate = new Java.Util.Date().Time;

			Button a = FindViewById <Button> (Resource.Id.signUpNextBtn);
			Button b = FindViewById <Button>(Resource.Id.signUpPreviousBtn);
			_viewPager = new CustomViewPager (this);
			_viewPager = FindViewById <CustomViewPager> (Resource.Id.signUpViewPager);
			_viewPager.Adapter = new CustomFragmentAdapter (SupportFragmentManager);
	
			b.Click += (object sender, EventArgs e) => 
			{
				_viewPager.SetCurrentItem (_viewPager.CurrentItem - 1, true);
			};
			a.Click += (object sender, System.EventArgs e) =>
				{
					_viewPager.SetCurrentItem (_viewPager.CurrentItem + 1, true);

					if (_viewPager.CurrentItem == 5)
					{
						if (username.Text != "" && password.Text != "" && confirm.Text != "" && name.Text != "" && address.Text != "")
						{
							var pb = new AlertDialog.Builder(this).Create();
							pb.SetView(new ProgressBar(this));
							pb.SetCancelable(false);
							RunOnUiThread(() => pb.Show());

							ThreadPool.QueueUserWorkItem(o =>
								{
									if (MainActivity.CIF.CreateUser(name.Text, address.Text, new DateTimeOffset(birthday.DateTime, new TimeSpan(0)), username.Text, password.Text, confirm.Text)) 
									{
										//Go to next menu (interests)	
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
								});
						}
						else
						{
							var errorDialog = new AlertDialog.Builder(this);
							errorDialog.SetMessage(Resources.GetString(Resource.String.sign_up_missing_info));
							errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => {});
							errorDialog.Show();
						}
					}
				};

			/*@continue.Click += (sender, e) =>
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
								if (MainActivity.CIF.CreateUser(name.Text, address.Text, new DateTimeOffset(birthday.DateTime, new TimeSpan(0)), username.Text, password.Text, confirm.Text))
									//Go to next menu (interests)

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
			};*/
		}
	}
}

