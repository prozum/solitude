using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using System.Threading;

namespace Solitude.Droid
{
	public class HostingFragment : TabFragment
	{
		public CoordinatorLayout Layout { get; set; }
		public ListView List { get; set; }
		public FloatingActionButton Fab { get; set; }
		public EventAdapter<Event> Adapter { get; set; }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.HostList, null)
							 .FindViewById<CoordinatorLayout>(Resource.Id.layout);
			List = Layout.FindViewById<ListView>(Resource.Id.list);
			Fab = Layout.FindViewById<FloatingActionButton>(Resource.Id.fab);
			Layout.RemoveAllViews();
			Layout.AddView(new ProgressBar(Activity));

			ThreadPool.QueueUserWorkItem(o =>
			{
				Adapter = new EventAdapter<Event>(Activity, MainActivity.CIF.GetHostedEvents(100));

				Adapter.OnUpdatePosition = (view, evnt, exp) =>
				{
					view.FindViewById<TextView>(Resource.Id.title).Text = evnt.Title;
					view.FindViewById<TextView>(Resource.Id.subtitle).Text = evnt.Date.ToString("G");

					view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
						string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}", 
									  evnt.Description, Resources.GetString(Resource.String.event_place), 
									  evnt.Location, Resources.GetString(Resource.String.event_participants), 
									  evnt.SlotsTaken, evnt.SlotsTotal);

					view.FindViewById<Button>(Resource.Id.action1).Text = GetString(Resource.String.cancel_button);
					view.FindViewById<Button>(Resource.Id.action2).Text = GetString(Resource.String.edit_button); ;
				};

				Adapter.OnAction1 = (i) =>
				{
					var alertBuilder = new Android.Support.V7.App.AlertDialog.Builder(Activity);
					alertBuilder.SetTitle(Resources.GetString(Resource.String.cancel_event));
					alertBuilder.SetMessage(Resources.GetString(Resource.String.message_cancel_event_confirm));

					alertBuilder.SetNegativeButton(Resources.GetString(Resource.String.no_abort), (s, e) => { });
					alertBuilder.SetPositiveButton(Resources.GetString(Resource.String.yes_cancel), (s, e) =>
					{
						var @event = Adapter.Items[i];
						MainActivity.CIF.DeleteEvent(@event);
						Adapter.RemoveAt(i);
						AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_canceled) + @event.Title, 2000).Show();
					});
					alertBuilder.Show();
                };
				Adapter.OnAction2 = EditEvent;
				Fab.Click += (s, e) => NewEvent();
				

				Activity.RunOnUiThread(() =>
				{
					List.Adapter = Adapter;
					Layout.RemoveAllViews();
					Layout.AddView(List);
					Layout.AddView(Fab);
				});
			});

			return Layout;
		}

		public override void OnSelected()
		{
			if (Layout != null)
			{
				Layout.RemoveAllViews();
				Layout.AddView(new ProgressBar(Activity));

				ThreadPool.QueueUserWorkItem(o =>
				{
					var events = MainActivity.CIF.GetHostedEvents(100);

					Activity.RunOnUiThread(() =>
					{
						Adapter.SetItems(events);
						Layout.RemoveAllViews();
						Layout.AddView(List);
						Layout.AddView(Fab);
					});
				});
			}
		}

		private void EditEvent(int i)
		{
			var @event = Adapter.Items[i];
			Intent intent = new Intent(Activity, typeof(HostEventActivity));
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
			intent.PutExtra("place", @event.Location);
			intent.PutExtra("maxslots", @event.SlotsTotal);
			intent.PutExtra("slotstaken", @event.SlotsTaken);
			intent.PutExtra("id", @event.Id);
			intent.PutExtra("index", (Activity as DrawerActivity).Position);
			intent.PutExtra("tab", Position);
			StartActivity(intent);
		}

		private void NewEvent()
		{
			var intent = new Intent(Activity, typeof(HostEventActivity));
			intent.PutExtra("type", "new");
			intent.PutExtra("index", (Activity as DrawerActivity).Position);
			intent.PutExtra("tab", Position);
			StartActivity(intent);
		}
	}
}