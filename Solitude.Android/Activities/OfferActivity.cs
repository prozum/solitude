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
	[Activity (Label = "Offer", Icon = "@drawable/Offer_Icon")]
	public class OfferActivity : DrawerActivity
	{
		protected OfferList Tilelist { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate (savedInstanceState);

			//Shows spinner to indicate loading
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();
					
					var offers = MainActivity.CIF.RequestOffers();
					var adapter = new OfferListAdapter(this, offers);
					Tilelist = new OfferList(this, adapter, (s, e) => AcceptOffer(), (s, e) => DeclineOffer());

					//Clear screen and show the found offers
					RunOnUiThread( () => 
						{
							ClearLayout();
							Content.AddView(Tilelist);
						});
				});
		}

		protected void AcceptOffer()
		{
			MainActivity.CIF.ReplyOffer(true, Tilelist.PopFocus());
		}

		protected void DeclineOffer()
		{
			MainActivity.CIF.ReplyOffer(false, Tilelist.PopFocus());
		}
	}
}