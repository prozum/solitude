
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
    /// <summary>
    /// This activity is used for hosting new activities, or editing existing activities.
    /// </summary>
	[Activity(Label = "Host Event")]			
	public class HostEventActivity : AppCompatActivity
	{
        /// <summary>
        /// Contains the different fragments for creating events
        /// </summary>
		protected EditEventAdapter Adapter { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SignUp);
			var finish = FindViewById<FloatingActionButton>(Resource.Id.finish);
			var viewpager = FindViewById<ViewPager>(Resource.Id.signUpViewPager);
			var progress = FindViewById<ProgressBar>(Resource.Id.signupProgress);
			Adapter = new EditEventAdapter(this, viewpager, finish,  progress);

            // Adds fragments to the adapter
			viewpager.Adapter = Adapter;
			viewpager.AddOnPageChangeListener(Adapter);

			Adapter.AddPager(new EventInfoFragment());
			Adapter.AddPager(new EventDateFragment());
			Adapter.AddPager(new EventTimeFragment());
		}
		
		public override void OnBackPressed()
		{
            // Allows the user to return to previous adapter when using hardware back button
			Adapter.PreviousPage();
		}

        /// <summary>
        /// Creates the arrows in the top allowing the user to go forward or backward, in addition to using swipe.
        /// </summary>
        /// <param name="menu">The current menu</param>
        /// <returns></returns>
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
}

