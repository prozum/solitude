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
	public class EventActivity : TileListActivity<EventListAdapter, Event>
	{
		public EventActivity() : base(Resource.Layout.ListActivity, Resource.Id.spinner1, Resource.Id.expandableListView1)
		{

		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			#region Finding listView item data
			#endregion

			// adapter for the spinner
			ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.SortSpinnerItems, Android.Resource.Layout.SimpleSpinnerItem);
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			Spinner.Adapter = adapter;

			// adapter for listView
			ListView.SetAdapter(new EventListAdapter (this, TestMaterial.Events, OnButton1));
		}
	}
}


