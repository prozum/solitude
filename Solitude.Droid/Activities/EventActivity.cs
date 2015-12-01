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

			var layout = LayoutInflater.Inflate(Resource.Layout.TapTest, null);
			var tablayout = layout.FindViewById<TabLayout>(Resource.Id.tab_layout);
			var viewpager = layout.FindViewById<CustomViewPager>(Resource.Id.view_pager);
			var adapter = new TabAdapter(this, viewpager, tablayout);

			adapter.AddTab(Resource.String.event_menu_recommended, new RecommendsFragment());
			adapter.AddTab(Resource.String.event_menu_joined, new AttendingFragment());
			adapter.AddTab(Resource.String.event_menu_hosted, new HostingFragment());

			tablayout.GetTabAt(Intent.GetIntExtra("tab", 0)).Select();
			SupportActionBar.Elevation = 0;
			Content.AddView(layout);
		}
	}
}
