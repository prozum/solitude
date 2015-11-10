using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace DineWithaDane.Android
{
	[Activity (Label = "Events")]
	public class EventActivity : DrawerActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			// setting up and drawer
			Position = 2;
			base.OnCreate (savedInstanceState);

			var adapter = new HostedEventListAdapter(this, new List<Event>());
			var tilelist = new HostedEventList(this, adapter, (s, e) => { }, (s, e) => { });
		}
	}
}
