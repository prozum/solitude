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
		public FrameLayout Layout { get; set; }
		public ListView List { get; set; }
		public EventAdapter<Event> Adapter { get; set; }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.EventList, null)
								 .FindViewById<FrameLayout>(Resource.Id.layout);
			List = Layout.FindViewById<ListView>(Resource.Id.list);
			Layout.RemoveAllViews();
			Layout.AddView(new ProgressBar(Activity));

			ThreadPool.QueueUserWorkItem(o =>
			{
				Adapter = new EventAdapter<Event>(Activity, MainActivity.CIF.GetJoinedEvents(100));

				Adapter.OnAction1 = (i) =>
				{
					var @event = Adapter.Items[i];
					MainActivity.CIF.CancelReg(@event);
					Adapter.RemoveAt(i);
					AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_left) + @event.Title, 2000).Show();
				};
				Adapter.OnUpdatePosition = (view, evnt, exp) =>
				{
					view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
						string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}",
									  evnt.Description, Resources.GetString(Resource.String.event_place),
									  evnt.Address, Resources.GetString(Resource.String.event_participants),
									  evnt.SlotsTaken, evnt.SlotsTotal);

					view.FindViewById<Button>(Resource.Id.action1).Text = GetString(Resource.String.leave_button);
					view.FindViewById<Button>(Resource.Id.action2).Visibility = ViewStates.Gone;
				};

				List.Adapter = Adapter;

				Activity.RunOnUiThread(() =>
				{

					Layout.RemoveAllViews();
					Layout.AddView(List);
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
					var events = MainActivity.CIF.GetJoinedEvents(100);

					Activity.RunOnUiThread(() =>
					{
						Adapter.SetItems(events);

						Layout.RemoveAllViews();
						Layout.AddView(List);
					});
				});
			}
		}
	}
}