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
	[Activity(Label = "Offer", Icon = "@drawable/Offer_Icon")]
	public class OfferActivity : DrawerActivity
	{
		protected OfferList Tilelist { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);

			//Shows spinner to indicate loading
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();
					
					var offers = MainActivity.CIF.RequestOffers();

					View view;

					if (offers.Count != 0)
					{
						var adapter = new OfferListAdapter(this, offers);
						Tilelist = new OfferList(this, adapter, (s, e) => AcceptOffer(), (s, e) => DeclineOffer());
						view = Tilelist;
					}
					else
					{
						view = new TextView(this);
						(view as TextView).Text = Resources.GetString(Resource.String.message_offer_nothing_here);
					}

					//Clear screen and show the found offers
					RunOnUiThread(() =>
						{
							ClearLayout();
							Content.AddView(view);
						});
				});
		}

		protected void AcceptOffer()
		{
			MainActivity.CIF.ReplyOffer(true, Tilelist.PopFocus());
			NoMoreOffers();
		}

		protected void DeclineOffer()
		{
			MainActivity.CIF.ReplyOffer(false, Tilelist.PopFocus());
			NoMoreOffers();
		}

		protected void NoMoreOffers()
		{
			if (Tilelist.Count == 0)
			{
				var text = new TextView(this);
				text.Text = Resources.GetString(Resource.String.message_offer_no_more);

				Content.RemoveAllViews();
				Content.AddView(text);
			}
		}
	}
}