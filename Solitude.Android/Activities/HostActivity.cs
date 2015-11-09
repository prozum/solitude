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

namespace DineWithaDane.Android
{
	[Activity (Label = "Hosts")]			
	public class HostActivity : DrawerActivity
	{
		ObservableCollection<Event> hostedEventsList;

		protected override void OnCreate (Bundle bundle)
		{
			// setting up and drawer
			Position = 3;
			base.OnCreate (bundle);

			hostedEventsList = new ObservableCollection<Event>();

			var content = FindViewById<FrameLayout>(Resource.Id.content_frame);
			var internalContent = new LinearLayout(this);
			internalContent.Orientation = Orientation.Vertical;
			content.AddView(internalContent);

			var metrics = new DisplayMetrics();
			WindowManager.DefaultDisplay.GetMetrics(metrics);

			var hostNewEventButton = new Button(this);
			hostNewEventButton.Text = "Host New Event";

			hostNewEventButton.Click += (object sender, EventArgs e) => StartActivity(typeof(CreateEventActivity));

			hostedEventsList.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => 
			{
				foreach (var item in hostedEventsList)
				{
					// Add element with event
				}
				content.AddView(hostNewEventButton);
			};

			foreach (var item in hostedEventsList)
			{
				// Add element with event
			}
			internalContent.AddView(hostNewEventButton);
		}
	}
}