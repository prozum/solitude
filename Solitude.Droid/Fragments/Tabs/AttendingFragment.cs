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
using System.Threading;

namespace Solitude.Droid
{
	public class AttendingFragment : TabFragment
	{
		protected List<Event> Events { get; set; }

		public AttendingFragment(List<Event> events)
		{
			Events = events;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.EventList, null);
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var adapter = new EventAdapter<Event>(Activity, Events);

			adapter.OnAction1 = (i) => adapter.RemoveAt(i);
			adapter.OnUpdatePosition = (view, evnt, exp) =>
			{
				view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
					string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}",
								  evnt.Description, Resources.GetString(Resource.String.event_place),
								  evnt.Address, Resources.GetString(Resource.String.event_participants),
								  evnt.SlotsTaken, evnt.SlotsTotal);

				view.FindViewById<Button>(Resource.Id.action1).Text = GetString(Resource.String.leave_button);
				view.FindViewById<Button>(Resource.Id.action2).Visibility = ViewStates.Gone;
			};

			list.Adapter = adapter;

			return layout;
		}
	}
}