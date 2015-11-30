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

namespace Solitude.Droid
{
	public class HostingFragment : TabFragment
	{
		protected EventAdapter<Event> Adapter { get; set; }
		protected List<Event> Events { get; set; }

		public HostingFragment(List<Event> events)
		{
			Events = events;
        }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.HostList, null);
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var @new = layout.FindViewById<FloatingActionButton>(Resource.Id.fab);

			Adapter = new EventAdapter<Event>(Activity, Events);

			Adapter.OnUpdatePosition = (view, evnt, exp) =>
			{
				view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
					string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}", 
								  evnt.Description, Resources.GetString(Resource.String.event_place), 
								  evnt.Address, Resources.GetString(Resource.String.event_participants), 
								  evnt.SlotsTaken, evnt.SlotsTotal);

				view.FindViewById<Button>(Resource.Id.action1).Text = "Cancel";
				view.FindViewById<Button>(Resource.Id.action2).Text = "Edit";
			};

			Adapter.OnAction1 = (i) => Adapter.RemoveAt(i);
			Adapter.OnAction2 = EditEvent;
			@new.Click += (s, e) => NewEvent();

			list.Adapter = Adapter;

			return layout;
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
			intent.PutExtra("place", @event.Address);
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