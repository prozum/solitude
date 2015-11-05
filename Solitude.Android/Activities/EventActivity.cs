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
			drawerPosition = 2;

			base.OnCreate (savedInstanceState);

			var adapter = new EventListAdapter(this, TestMaterial.Events);
			var tilelist = new EventList(this, adapter);

			Content.AddView(tilelist);
		}
	}
}


