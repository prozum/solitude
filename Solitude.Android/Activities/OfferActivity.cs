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
	public class OfferActivity : AbstractActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			drawerPosition = 1;

			base.OnCreate (savedInstanceState);

			var adapter = new OfferListAdapter(this, TestMaterial.Events);
			var tilelist = new OfferList(this, adapter);

			Content.AddView(tilelist);
		}
	}
}


