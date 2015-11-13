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
	[Activity (Label = "Events", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected JoinedEventList Tilelist { get; set; }

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

					var events = MainActivity.CIF.GetJoinedEvents(100);
					var adapter = new JoinedEventListAdapter(this, events);
					Tilelist = new JoinedEventList(this, adapter, (s, e) => LeaveEvent());

					//Clear screen and show the found offers
					RunOnUiThread( () => 
						{
							ClearLayout();
							Content.AddView(Tilelist);
						});
				});
		}

		protected void LeaveEvent()
		{
			MainActivity.CIF.CancelReg(Tilelist.PopFocus());
		}
	}
}
