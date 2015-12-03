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
using System.Collections.Generic;
using Android.Support.Design.Widget;
using Android.Support.V7.App;

namespace Solitude.Droid
{
	[Activity(Label = "@string/label_signup")]
	public class SignUpActivity : AppCompatActivity
	{
		protected SignUpAdapter Adapter { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SignUp);
			var finish = FindViewById<FloatingActionButton>(Resource.Id.finish);
			var viewpager = FindViewById<ViewPager>(Resource.Id.signUpViewPager);
			var progress = FindViewById<ProgressBar>(Resource.Id.signupProgress);
			Adapter = new SignUpAdapter(this, viewpager, finish, progress);

			viewpager.Adapter = Adapter;
			viewpager.AddOnPageChangeListener(Adapter);

			Adapter.AddPager(new SignUpNameAddress());
			Adapter.AddPager(new SignUpBirthDay());
			Adapter.AddPager(new SignUpInfo(InfoType.Interest, GetString(Resource.String.sign_up_interests)));
			Adapter.AddPager(new SignUpInfo(InfoType.Language, GetString(Resource.String.sign_up_languages)));
			Adapter.AddPager(new SignUpInfo(InfoType.FoodHabit, GetString(Resource.String.sign_up_foodpreferences)));
			Adapter.AddPager(new SignUpUsernamePassword());
		}

		public override void OnBackPressed()
		{
			Adapter.PreviousPage();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			var prev = menu.Add(0, 0, 0, "Prev").SetIcon(Resource.Drawable.ic_arrow_back_black_24dp);
			var next = menu.Add(0, 1, 0, "Next").SetIcon(Resource.Drawable.ic_arrow_forward_black_24dp);
			next.SetShowAsAction(ShowAsAction.IfRoom);
			prev.SetShowAsAction(ShowAsAction.IfRoom);

			Adapter.SetNextButton(next);
			Adapter.SetPreviousButton(prev);

			return true;
		}
	}

		/*
		[Activity(Label = "Solitude.Android")]
		public class SignUpActivity : FragmentActivity
		{
			public static CustomViewPager _viewPager;
			private SignUpFragmentNameAddress nameAdd= new SignUpFragmentNameAddress();
			private string username, password, confirm, name, address;
			List<InfoChange> InterestList, FoodPreferenceList, LanguageList;
			private DateTime birthdate;

			protected override void OnCreate(Bundle savedInstanceState)
			{
				//Setup
				base.OnCreate(savedInstanceState);
				SetContentView(Resource.Layout.SignUp);

				//Finds all widgets on the SignUp-layout
				View fragView4 = View.Inflate(this, Resource.Layout.signupFragLayout4, null);

				//Initialize ViewPager
				_viewPager = FindViewById <CustomViewPager> (Resource.Id.signUpViewPager);
				_viewPager.Adapter = new CustomFragmentAdapter (SupportFragmentManager);
				_viewPager.OnPageLeft += saveFragmentInfo;
				_viewPager.PageScrolled += updateProgress;

				//Initialize the progress-bar
				var bar = FindViewById<ProgressBar>(Resource.Id.signupProgress);
				bar.Max = 5;
				bar.Progress = 1;

				_viewPager.PageSelected += (sender, e) => 
					{
						if (e.Position == (int) CustomFragmentAdapter.CurrentlyShown.Interests) {
							InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
							imm.HideSoftInputFromWindow(_viewPager.WindowToken, 0);
						}
					};
			}

			private void updateProgress(object sender, ViewPager.PageScrolledEventArgs e)
			{
				var bar = FindViewById<ProgressBar>(Resource.Id.signupProgress);
				bar.Progress = e.Position + 1;
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
				else if (e.fragment is SignUpFragmentInterests)
				{
					var frag = e.fragment as SignUpFragmentInterests;
					InterestList = frag.SaveInfo();
				}
				else if (e.fragment is SignUpFragmentFoodPreferences)
				{
					var frag = e.fragment as SignUpFragmentFoodPreferences;
					FoodPreferenceList = frag.SaveInfo();
				}
				else if (e.fragment is SignUpFragmentLanguages)
				{
					var frag = e.fragment as SignUpFragmentLanguages;
					LanguageList = frag.SaveInfo();
				}
			}

			public void confirmSignup(object sender, EventArgs e)
			{
				var frag = ((_viewPager.Adapter as CustomFragmentAdapter).CurrentItem as SignUpFragmentUsernamePassword);
				username = frag.Username;
				password = frag.Password;
				confirm = frag.ConfirmedPassword;

				if (username != "" && password != "" && confirm != "" && name != "" && address != "")
				{
					//Generates a dialog showing a spinner
					var pb = new Android.Support.V7.App.AlertDialog.Builder(this).Create();
					pb.SetView(new ProgressBar(pb.Context));
					pb.SetCancelable(false);
					RunOnUiThread(() => pb.Show());

					ThreadPool.QueueUserWorkItem(o =>
						{
							//Tries to create the user on the server
							if (MainActivity.CIF.CreateUser(name, address, new DateTimeOffset(birthdate, new TimeSpan(0)), username, password, confirm)) 
							{
								MainActivity.CIF.Login(username, password);
								foreach (var interest in InterestList) {
									MainActivity.CIF.AddInformation(interest);
								}
								foreach (var foodPreference in FoodPreferenceList) {
									MainActivity.CIF.AddInformation(foodPreference);
								}
								foreach (var language in LanguageList) {
									MainActivity.CIF.AddInformation(language);
								}

								Intent profileIntent = new Intent(this, typeof(ProfileActivity));
								StartActivity(profileIntent);
								Finish();
							}
							else
							{
								//Generates a dialog showing an errormessage
								var errorDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
								errorDialog.SetMessage(MainActivity.CIF.LatestError);

								//Goes back to the initial signup-screen
								errorDialog.SetNeutralButton(Resource.String.ok, (s, earg) => 
									_viewPager.CurrentItem = (int) CustomFragmentAdapter.CurrentlyShown.NameAddress );
								RunOnUiThread(() => errorDialog.Show());
							}

							//Removes the spinner again
							RunOnUiThread(() => pb.Dismiss());
						});
				}
				else
				{
					//Generates an error dialog showing that some information is missing
					var errorDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
					errorDialog.SetMessage(Resources.GetString(Resource.String.sign_up_missing_info));
					errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => {
						RunOnUiThread(() => _viewPager.CurrentItem = (int) CustomFragmentAdapter.CurrentlyShown.NameAddress );
					});
					errorDialog.Show();
				}
			}
		}
	/**/
}

