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
using Android.Views.InputMethods;
using Android.Graphics;
using System.Text.RegularExpressions;

namespace Solitude.Droid
{
	[Activity(Label = "Solitude.Android")]
	public class SignUpActivity : FragmentActivity
	{
		public static CustomViewPager _viewPager;
		private SignUpFragmentNameAddress nameAdd= new SignUpFragmentNameAddress();
		private string username, password, confirm, name, address;
		private DateTime birthdate;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			//Setup
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SignUp);

			//Finds all widgets on the SignUp-layout
			View fragView4 = View.Inflate(this, Resource.Layout.signupFragLayout4, null);
			View fragView5 = View.Inflate(this, Resource.Layout.signupFragLayout5, null);

			//Finds the buttons that switches pages
			Button next = FindViewById <Button> (Resource.Id.signUpNextBtn);

			//Initialize ViewPager
			_viewPager = new CustomViewPager (this);
			_viewPager = FindViewById <CustomViewPager> (Resource.Id.signUpViewPager);
			_viewPager.Adapter = new CustomFragmentAdapter (SupportFragmentManager);
			_viewPager.OnPageLeft += saveFragmentInfo;

			next.Click += nextClicked;
		}

		private void nextClicked(object sender, EventArgs e)
		{
			//A bool that determines if the user is ready to move on to the next page
			bool moveOn = true;

			//Submits the user after username and password has been filled
			if ((_viewPager.Adapter as CustomFragmentAdapter).CurrentItem.GetType() == typeof(SignUpFragmentUsernamePassword))
			{
				moveOn = false;

				if (username != "" && password != "" && confirm != "" && name != "" && address != "")
				{
					//Generates a dialog showing a spinner
					var pb = new AlertDialog.Builder(this).Create();
					pb.SetView(new ProgressBar(this));
					pb.SetCancelable(false);
					RunOnUiThread(() => pb.Show());

					ThreadPool.QueueUserWorkItem(o =>
						{
							//Tries to create the user on the server
							if (MainActivity.CIF.CreateUser(name, address, new DateTimeOffset(birthdate, new TimeSpan(0)), username, password, confirm)) 
							{
								//Moves on to the next page
								moveOn = true;
								RunOnUiThread( () => _viewPager.Next());
							}
							else
							{
								//Generates a dialog showing an errormessage
								var errorDialog = new AlertDialog.Builder(this);
								errorDialog.SetMessage(MainActivity.CIF.LatestError);
								errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => { _viewPager.SetCurrentItem((int) CustomFragmentAdapter.CurrentlyShown.NameAddress, false); });
								RunOnUiThread(() => errorDialog.Show());
							}

							//Removes the spinner again
							RunOnUiThread(() => pb.Dismiss());
						});
				}
				else
				{
					//Generates an error dialog showing that some information is missing
					var errorDialog = new AlertDialog.Builder(this);
					errorDialog.SetMessage(Resources.GetString(Resource.String.sign_up_missing_info));
					errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => {});
					errorDialog.Show();
				}
			}
			else if(moveOn)
			{
				//Switches pages and hides keyboard on click
				_viewPager.Next();
				InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
			}
		}

		/// <summary>
		/// Saves the fragment info before changing to the next.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void saveFragmentInfo (object sender, FragmentEventArgs e)
		{
			if (e.fragment is SignUpFragmentNameAddress)
			{
				var frag = e.fragment as SignUpFragmentNameAddress;
				name = frag.Name;
				address = frag.Address;
				birthdate = frag.Birthdate;
			}
			else if (e.fragment is SignUpFragmentUsernamePassword)
			{
				var frag = e.fragment as SignUpFragmentUsernamePassword;
				username = frag.Username;
				password = frag.Password;
				confirm = frag.ConfirmedPassword;
			}
			else if (e.fragment is SignUpFragmentInterests)
			{

			}
			else if (e.fragment is SignUpFragmentFoodPreferences)
			{

			}
			else if (e.fragment is SignUpFragmentLanguages)
			{

			}

		}
	}
}

