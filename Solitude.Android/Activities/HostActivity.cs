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

namespace DineWithaDane.Droid
{
	[Activity(Label = "Host", Icon = "@drawable/Host_Icon")]			
	public class HostActivity : DrawerActivity
	{
		protected RelativeLayout InternalContent { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			// setting up and drawer
			base.OnCreate(bundle);

			Button b = new Button(this);
			b.Text = Resources.GetString(Resource.String.app_name);

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

					View view = null;
					var viewparams = new RelativeLayout.LayoutParams(-1, -2);

					if (events.Count != 0)
					{
					}
					else
					{
						view = new TextView(this);
						(view as TextView).Text = Resources.GetString(Resource.String.message_host_nothing_hosted);
					}


					InternalContent = new RelativeLayout(this);

					var hostNewEventButton = new Button(this);
					var hostbuttonparams = new RelativeLayout.LayoutParams(-1, -2);
					hostNewEventButton.Text = Resources.GetString(Resource.String.host_new_event);

					hostNewEventButton.Click += (object sender, EventArgs e) =>
					{
						var intent = new Intent(this, typeof(HostEventActivity));
						intent.PutExtra("type", "new");
						StartActivity(intent);
					};

					hostNewEventButton.Id = 30;
					viewparams.AddRule(LayoutRules.Above, hostNewEventButton.Id);
					viewparams.AddRule(LayoutRules.AlignParentTop);
					hostbuttonparams.AddRule(LayoutRules.AlignParentBottom);

					RunOnUiThread(() =>
						{
							ClearLayout();
							Content.AddView(InternalContent);
							InternalContent.AddView(view);
							InternalContent.AddView(hostNewEventButton);
							hostNewEventButton.LayoutParameters = hostbuttonparams;
							view.LayoutParameters = viewparams;
						});
				});
		}

		protected void DeleteEvent()
		{
			var alertBuilder = new AlertDialog.Builder(this);
			var alert = alertBuilder.Create();
			alert.SetTitle(Resources.GetString(Resource.String.cancel_event));
			alert.SetMessage(Resources.GetString(Resource.String.message_cancel_event_confirm));
			alert.SetButton2(Resources.GetString(Resource.String.no_abort), (object senders, DialogClickEventArgs ev) => alert.Dismiss());
			alert.SetButton(Resources.GetString(Resource.String.yes_cancel), (object senders, DialogClickEventArgs ev) =>
				{
					//MainActivity.CIF.DeleteEvent(Tilelist.PopFocus());
					/*
					if (Tilelist.Count == 0)
					{
						var text = new TextView(this);
						text.Text = Resources.GetString(Resource.String.message_host_nothing_hosted);

						InternalContent.RemoveView(Tilelist);
						InternalContent.AddView(text);
					}
					*/
				});
			alert.Show();
		}

		protected void EditEvent()
		{
			/*
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
			intent.PutExtra("place", @event.Address);
			intent.PutExtra("maxslots", @event.SlotsTotal);
			intent.PutExtra("leftslots", @event.SlotsTaken);
			intent.PutExtra("id", @event.Id);
			StartActivity(intent);
			*/
		}
	}
}