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
	[Activity (Label = "Offers")]
	public class OfferActivity : DrawerActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			var adapter = new OfferListAdapter(this, TestMaterial.Events);
			var tilelist = new OfferList(this, adapter);

			// setting up and drawer
			Position = 1;
			base.OnCreate (savedInstanceState);

			// adding tilelist to activity
			Content.AddView(tilelist);
		}
	}
}