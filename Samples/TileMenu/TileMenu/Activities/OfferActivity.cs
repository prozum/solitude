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
	public class OfferActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			var adapter = new OfferListAdapter(this, TestMaterial.Events);
			var tilelist = new OfferList(this, adapter);

			SetContentView(tilelist, new ViewGroup.LayoutParams(-1,-1));
		}
	}
}


