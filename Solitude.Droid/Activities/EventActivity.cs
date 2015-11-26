using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace Solitude.Droid
{
	[Activity(Label = "@string/label_eventactivity", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);

		}

		protected override void OnResume()
		{
			base.OnResume();

			ClearLayout();
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
			{
				var offers = MainActivity.CIF.RequestOffers();
				var events = MainActivity.CIF.GetJoinedEvents(100);
				var hosted = MainActivity.CIF.GetHostedEvents(100);

				var layout = LayoutInflater.Inflate(Resource.Layout.TapTest, null);
				var tablayout = layout.FindViewById<TabLayout>(Resource.Id.tab_layout);
				var viewpager = layout.FindViewById<ViewPager>(Resource.Id.view_pager);

				var adapter = new TabAdapter(this, viewpager, tablayout);
				tablayout.SetOnTabSelectedListener(adapter);

				viewpager.SetCurrentItem(Intent.GetIntExtra("tab", 0), false);

				adapter.AddTab(Resource.String.event_menu_recommended, new RecommendsFragment(offers));
				adapter.AddTab(Resource.String.event_menu_joined, new AttendingFragment(events));
				adapter.AddTab(Resource.String.event_menu_hosted, new HostingFragment(hosted));

				//Clear screen and show the found offers
				RunOnUiThread(() =>
				{
					ClearLayout();
					SupportActionBar.Elevation = 0;
					Content.AddView(layout);
				});
			});
		}
	}
}
