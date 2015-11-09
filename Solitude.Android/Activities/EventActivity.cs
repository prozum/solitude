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
	[Activity (Label = "Events")]
	public class EventActivity : DrawerActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			// setting up and drawer
			Position = 2;
			base.OnCreate (savedInstanceState);

			//Show spinner to indicate loading
			showSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch events from server
					Looper.Prepare();
					var events = MainActivity.CIF.GetOwnEvents(100);
					var adapter = new EventListAdapter(this, MainActivity.CIF.GetOwnEvents(100));
					var tilelist = new EventList(this, adapter);

					//Clear screen and show found events
					RunOnUiThread( () => {
						clearLayout();

						//If no events are found display an error-message
						if(events == null){
							var dialog = new AlertDialog.Builder(this);
							dialog.SetMessage("Sorry, couldn't fetch events:\n" + MainActivity.CIF.LatestError);
							dialog.Show();
						}

						// adding tilelist to activity
						Content.AddView(tilelist);
					});
				});
		}
	}
}
