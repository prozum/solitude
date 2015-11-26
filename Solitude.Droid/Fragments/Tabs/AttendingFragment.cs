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
	public class AttendingFragment : Android.Support.V4.App.Fragment
	{
		protected List<Event> Events { get; set; }

		public AttendingFragment(List<Event> events)
		{
			Events = events;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

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
					string.Format("{0}\n\n{1}\n{2}/{3}", evnt.Description, evnt.Address, evnt.SlotsTaken, evnt.SlotsTotal);

				view.FindViewById<Button>(Resource.Id.action1).Text = "Leave";
				view.FindViewById<Button>(Resource.Id.action2).Visibility = ViewStates.Gone;
			};

			list.Adapter = adapter;

			return layout;
		}
	}
}