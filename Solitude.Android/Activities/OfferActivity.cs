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
	[Activity (Label = "Offer")]
	public class OfferActivity : DrawerActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			// setting up and drawer
			Position = 1;
			base.OnCreate (savedInstanceState);

			//Shows spinner to indicate loading
			showSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					prepareLooper();
					
					var offers = MainActivity.CIF.RequestOffers();
					var adapter = new OfferListAdapter(this, offers);
					var tilelist = new OfferList(this, adapter);

					//Clear screen and show the found offers
					RunOnUiThread( () => {
						clearLayout();

						Content.AddView(tilelist);
					});
				});
		}
	}
}