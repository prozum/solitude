using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using System.Threading;

namespace DineWithaDane.Android
{
	[Activity (Label = "Host")]			
	public class HostActivity : DrawerActivity
	{
		ObservableCollection<Event> hostedEventsList;

		protected override void OnCreate (Bundle bundle)
		{
			// setting up and drawer
			Position = 3;
			base.OnCreate (bundle);

			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					PrepareLooper();

					var events = MainActivity.CIF.GetOwnEvents(100);
					var adapter = new EventListAdapter(this, events);
					var tilelist = new EventList(this, adapter);

					var content = FindViewById<FrameLayout>(Resource.Id.content_frame);
					var internalContent = new LinearLayout(this);
					internalContent.Orientation = Orientation.Vertical;

					var hostNewEventButton = new Button(this);
					hostNewEventButton.Text = "Host New Event";

					hostNewEventButton.Click += (object sender, EventArgs e) => {
						var intent = new Intent(this, typeof(HostEventActivity));
						intent.PutExtra("type", "new");
						StartActivity(intent);
					};

					RunOnUiThread (() =>
						{
							ClearLayout();
							content.AddView(internalContent);
							internalContent.AddView(hostNewEventButton);
							internalContent.AddView(tilelist);
						});

					/*hostedEventsList.CollectionChanged += (sender, e) =>
					{
						foreach (var item in hostedEventsList)
						{
							// Add element with event
						}
						content.AddView(hostNewEventButton);
					};*/

					/*foreach (var item in events)
					{
						LinearLayout hostEventLayout = new LinearLayout(this);
						hostEventLayout.Orientation = Orientation.Horizontal;

						TextView eventTitle = new TextView(this);
						eventTitle.Text = item.Title;

						TextView eventDate = new TextView(this);
						eventDate.Text = item.Date.ToString();

						TextView eventDescription = new TextView(this);
						eventDescription.Text = item.Description;

						RunOnUiThread( () => {
							hostEventLayout.AddView(eventTitle);
							hostEventLayout.AddView(eventDate);
							hostEventLayout.AddView(eventDescription);
						});

						hostEventLayout.Click += (object sender, EventArgs e) =>
						{
							Intent intent = new Intent(this, typeof(HostEventActivity));
							intent.PutExtra("type", "edit");
							intent.PutExtra("title", item.Title);
							intent.PutExtra("description", item.Description);
							intent.PutExtra("date day", item.Date.Day);
							intent.PutExtra("date month", item.Date.Month);
							intent.PutExtra("date year", item.Date.Year);
							intent.PutExtra("date hour", item.Date.Hour);
							intent.PutExtra("date minutte", item.Date.Minute);
							intent.PutExtra("place", item.Place);
							intent.PutExtra("maxslots", item.MaxSlots);
							StartActivity(intent);
						};
					}*/
				});

			//var metrics = new DisplayMetrics();
			//WindowManager.DefaultDisplay.GetMetrics(metrics);

		}
	}
}