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
	[Activity(Label = "Events", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);
			SupportActionBar.Elevation = 0;
			var layout = LayoutInflater.Inflate(Resource.Layout.TapTest, null);
			var tablayout = layout.FindViewById<TabLayout>(Resource.Id.tab_layout);
			var viewpager = layout.FindViewById<ViewPager>(Resource.Id.view_pager);

			var adapter = new TabAdapter(this, viewpager, tablayout);
			tablayout.SetOnTabSelectedListener(adapter);

			adapter.AddTab(Resource.String.event_menu_recommended, new RecommendsFragment());
			adapter.AddTab(Resource.String.event_menu_joined, new AttendingFragment());
			adapter.AddTab(Resource.String.event_menu_hosted, new HostingFragment());

			viewpager.SetCurrentItem(Intent.GetIntExtra("tab", 0), false);

			Content.AddView(layout);
			/*
			//Shows spinner to indicate loading
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();

					var events = MainActivity.CIF.GetJoinedEvents(100);

					View view;

					if (events.Count != 0)
					{
						var adapter = new JoinedEventListAdapter(this, events);
						Tilelist = new JoinedEventList(this, adapter, (s, e) => LeaveEvent());
						view = Tilelist;
					}
					else
					{
						view = new TextView(this);
						(view as TextView).Text = Resources.GetString(Resource.String.message_event_nothing_here);
					}

					//Clear screen and show the found offers
					RunOnUiThread(() =>
						{
							ClearLayout();
							Content.AddView(view);
						});
				});
			/**/
		}
	}
}
