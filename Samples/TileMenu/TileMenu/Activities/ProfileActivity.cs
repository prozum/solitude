
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TileMenu
{
	[Activity(Label = "ProfileActivity")]			
	public class ProfileActivity : TileListActivity<OfferListAdapter, Event>
	{
		public ProfileActivity() : base(Resource.Layout.ListActivity, Resource.Id.List2)
		{

		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			#region Finding listView item data
			#endregion

			// adapter for listView
			ListView.SetAdapter(new EventListAdapter (this, TestMaterial.Events, OnButton1));
		}
	}
}

