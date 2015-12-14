using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Android.Support.V7.App;

namespace Solitude.Droid
{
    [Activity(Label = "@string/label_signup")]
	public class SignUpActivity : AppCompatActivity
	{
        /// <summary>
        /// Contains the fragments used for signing up
        /// </summary>
		protected SignUpAdapter Adapter { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SignUp);
			var finish = FindViewById<FloatingActionButton>(Resource.Id.finish);
			var viewpager = FindViewById<ViewPager>(Resource.Id.signUpViewPager);
			var progress = FindViewById<ProgressBar>(Resource.Id.signupProgress);
			Adapter = new SignUpAdapter(this, viewpager, finish, progress);

            // Adds fragments to the adapter
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
			Adapter.PreviousPage(); // Allows the user to go back when pressing the hardware back button
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
            // Sets next and previos arrows in the top of the page.
			var prev = menu.Add(0, 0, 0, "Prev").SetIcon(Resource.Drawable.ic_arrow_back_black_24dp);
			var next = menu.Add(0, 1, 0, "Next").SetIcon(Resource.Drawable.ic_arrow_forward_black_24dp);
			next.SetShowAsAction(ShowAsAction.IfRoom);
			prev.SetShowAsAction(ShowAsAction.IfRoom);

			Adapter.SetNextButton(next);
			Adapter.SetPreviousButton(prev);

			return true;
		}
	}
}

