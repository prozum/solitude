﻿using System;
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
	[Activity (Label = "Events", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected JoinedEventList Tilelist { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			// setting up and drawer
			Position = 2;
			base.OnCreate (savedInstanceState);

			//Shows spinner to indicate loading
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();

					//var offers = MainActivity.CIF.RequestOffers();
					var adapter = new JoinedEventListAdapter(this, new List<Event>());
					Tilelist = new JoinedEventList(this, adapter, LeaveEvent);

					//Clear screen and show the found offers
					RunOnUiThread( () => 
						{
							ClearLayout();
							Content.AddView(Tilelist);
						});
				});
		}

		protected void LeaveEvent(object sender, EventArgs e)
		{
			MainActivity.CIF.CancelReg(Tilelist.GetFocus());
			Tilelist.RemoveFocus();
		}
	}
}
