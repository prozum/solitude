using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Android
{
	[Activity (Label = "Events")]
	public class EventActivity : AbstractActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			var adapter = new EventListAdapter(this, MainActivity.CIF.GetOwnEvents(100));
			var tilelist = new EventList(this, adapter);

			// setting up and drawer
			drawerPosition = 2;
			base.OnCreate (savedInstanceState);

			// adding tilelist to activity
			Content.AddView(tilelist);
		}
	}
}
