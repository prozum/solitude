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
	[Activity(Label = "Host", Icon = "@drawable/Host_Icon")]			
	public class HostActivity : DrawerActivity
	{
		protected HostedEventList Tilelist { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			// setting up and drawer
			base.OnCreate(bundle);

			// See OnResume for rest of content
		}

		protected override void OnResume()
		{
			base.OnResume();

			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					PrepareLooper();

					var events = MainActivity.CIF.GetHostedEvents(100);
					var adapter = new HostedEventListAdapter(this, events);
					Tilelist = new HostedEventList(this, adapter, (s, e) => DeleteEvent(), (s, e) => EditEvent());
					var tilelistparams = new RelativeLayout.LayoutParams(-1, -2);

					var internalContent = new RelativeLayout(this);

					var hostNewEventButton = new Button(this);
					var hostbuttonparams = new RelativeLayout.LayoutParams(-1, -2);
					hostNewEventButton.Text = "Host New Event";

					hostNewEventButton.Click += (object sender, EventArgs e) =>
					{
						var intent = new Intent(this, typeof(HostEventActivity));
						intent.PutExtra("type", "new");
						StartActivity(intent);
					};

					hostNewEventButton.Id = 30;
					tilelistparams.AddRule(LayoutRules.Above, hostNewEventButton.Id);
					tilelistparams.AddRule(LayoutRules.AlignParentTop);
					hostbuttonparams.AddRule(LayoutRules.AlignParentBottom);

					RunOnUiThread(() =>
						{
							ClearLayout();
							Content.AddView(internalContent);
							internalContent.AddView(Tilelist);
							internalContent.AddView(hostNewEventButton);
							hostNewEventButton.LayoutParameters = hostbuttonparams;
							Tilelist.LayoutParameters = tilelistparams;
						});
				});
		}

		protected void DeleteEvent()
		{
			var alertBuilder = new AlertDialog.Builder(this);
			var alert = alertBuilder.Create();
			alert.SetTitle("Cancel Event");
			alert.SetMessage("Are you sure you want to cancel the event? You wont be able to recover the event afterwards.");
			alert.SetButton2("No, Abort", (object senders, DialogClickEventArgs ev) => alert.Dismiss());
			alert.SetButton("Yes, Cancel", (object senders, DialogClickEventArgs ev) =>
				{
					MainActivity.CIF.DeleteEvent(Tilelist.PopFocus());
				});
			alert.Show();
		}

		protected void EditEvent()
		{
			Event @event = Tilelist.GetFocus();
			Intent intent = new Intent(this, typeof(HostEventActivity));
			// All event information must be passed to the intent.
			// Only alternative is a static var with a reference to the event.
			intent.PutExtra("type", "edit");
			intent.PutExtra("title", @event.Title);
			intent.PutExtra("description", @event.Description);
			intent.PutExtra("date day", @event.Date.Day);
			intent.PutExtra("date month", @event.Date.Month);
			intent.PutExtra("date year", @event.Date.Year);
			intent.PutExtra("date hour", @event.Date.Hour);
			intent.PutExtra("date minutte", @event.Date.Minute);
			intent.PutExtra("place", @event.Place);
			intent.PutExtra("maxslots", @event.MaxSlots);
			intent.PutExtra("leftslots", @event.SlotsLeft);
			intent.PutExtra("id", @event.ID);
			StartActivity(intent);
		}
	}
}