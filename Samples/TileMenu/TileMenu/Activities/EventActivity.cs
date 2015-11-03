using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TileMenu
{
	[Activity (Label = "TileMenu", MainLauncher = true)]
	public class EventActivity : Activity
	{
		public SortableTileList<Event> EventList
		{
			get;
			set;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			var adapter = new EventListAdapter(this, TestMaterial.Events);
			var tilelist = new EventList(this, adapter);

			SetContentView(tilelist, new ViewGroup.LayoutParams(-1,-1));
		}


	}
}


