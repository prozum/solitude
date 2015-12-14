
using Android.App;
using Android.Content;
using Android.Views;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace Solitude.Droid
{
    /// <summary>
    /// The EventActivity contains all events. The activity is split up in three sections by tabs.
    /// </summary>
	[Activity(Label = "@string/label_eventactivity", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);

			var layout = LayoutInflater.Inflate(Resource.Layout.TapTest, null);
			var tablayout = layout.FindViewById<TabLayout>(Resource.Id.tab_layout);
			var viewpager = layout.FindViewById<ViewPager>(Resource.Id.view_pager);
			var adapter = new TabAdapter(this, viewpager, tablayout);

            // Sets the content of the tabs
			adapter.AddTab(Resource.String.event_menu_recommended, new RecommendsFragment());
			adapter.AddTab(Resource.String.event_menu_joined, new AttendingFragment());
			adapter.AddTab(Resource.String.event_menu_hosted, new HostingFragment());

			tablayout.GetTabAt(Intent.GetIntExtra("tab", 0)).Select();
			SupportActionBar.Elevation = 0; // Removes the shadow between the actionbar and tab bar, as per Google Material Design Guidelines.
			Content.AddView(layout);
		}
	}
}
