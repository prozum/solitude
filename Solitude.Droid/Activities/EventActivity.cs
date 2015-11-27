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
		protected View Layout { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);

			Layout = LayoutInflater.Inflate(Resource.Layout.TapTest, null);
		}

		protected override void OnResume()
		{
			base.OnResume();

			ThreadPool.QueueUserWorkItem(o =>
			{
				RunOnUiThread(() =>
				{
					ClearLayout();
					ShowSpinner();
				});

				var offers = MainActivity.CIF.RequestOffers();
				var events = MainActivity.CIF.GetJoinedEvents(100);
				var hosted = MainActivity.CIF.GetHostedEvents(100);

				RunOnUiThread(() =>
				{
					var tablayout = Layout.FindViewById<TabLayout>(Resource.Id.tab_layout);
					var viewpager = Layout.FindViewById<CustomViewPager>(Resource.Id.view_pager);
					viewpager.ScrollingEnabled = false;

					var adapter = new TabAdapter(this, viewpager, tablayout);
					viewpager.Adapter = adapter;
                    tablayout.SetOnTabSelectedListener(adapter);

					adapter.AddTab(Resource.String.event_menu_recommended, new RecommendsFragment(offers));
					adapter.AddTab(Resource.String.event_menu_joined, new AttendingFragment(events));
					adapter.AddTab(Resource.String.event_menu_hosted, new HostingFragment(hosted));

					ClearLayout();
					SupportActionBar.Elevation = 0;
					Content.AddView(Layout);
				});
			});
		}
	}
}
