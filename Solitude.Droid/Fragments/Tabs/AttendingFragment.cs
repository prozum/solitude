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
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	public class AttendingFragment : TabFragment
	{
		public FrameLayout Layout { get; set; }
		public ListView List { get; set; }
		public EventAdapter<Event> Adapter { get; set; }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			Layout = inflater.Inflate(Resource.Layout.EventList, null)
						     .FindViewById<FrameLayout>(Resource.Id.layout);
			List = Layout.FindViewById<ListView>(Resource.Id.list);

			// Show spinner
			Layout.RemoveAllViews();
			Layout.AddView(new ProgressBar(Activity));

			ThreadPool.QueueUserWorkItem(o =>
			{
				// Get joined events and creat adapter.
				Adapter = new EventAdapter<Event>(Activity, MainActivity.CIF.GetJoinedEvents(100));

				// On leave click
				Adapter.OnAction1 = (i) =>
				{
					// Build an alertdialog
					var alertBuilder = new Android.Support.V7.App.AlertDialog.Builder(Activity);
					alertBuilder.SetTitle(Resources.GetString(Resource.String.leave_event));
					alertBuilder.SetMessage(Resources.GetString(Resource.String.message_leave_event_confirm));

					alertBuilder.SetNegativeButton(Resources.GetString(Resource.String.no), (s, e) => { });
					alertBuilder.SetPositiveButton(Resources.GetString(Resource.String.yes), (s, e) =>
					{
						var @event = Adapter.Items[i];

						//Leave and remove event.
						MainActivity.CIF.CancelReg(@event);
						Adapter.RemoveAt(i);

						// Show update snackbar
						AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_left) + @event.Title, 2000).Show();
					});

					// Show alertdialog
					alertBuilder.Show();
				};
				Adapter.OnUpdatePosition = (view, evnt, exp) =>
				{
					view.FindViewById<TextView>(Resource.Id.title).Text = evnt.Title;
					view.FindViewById<TextView>(Resource.Id.subtitle).Text = evnt.Date.ToString("G");

					// Content of card contains description, location, slotstaken and slotstotal.
					view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
						string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}",
									  evnt.Description, Resources.GetString(Resource.String.event_place),
									  evnt.Location, Resources.GetString(Resource.String.event_participants),
									  evnt.SlotsTaken, evnt.SlotsTotal);

					view.FindViewById<Button>(Resource.Id.action1).Text = GetString(Resource.String.leave_button);

					// Remove second button.
					view.FindViewById<Button>(Resource.Id.action2).Visibility = ViewStates.Gone;
				};


				Activity.RunOnUiThread(() =>
				{
					// Set listview adapter
					List.Adapter = Adapter;

					// Remove spinner, and add listview
					Layout.RemoveAllViews();
					Layout.AddView(List);
				});
			});

			return Layout;
		}

		/// <summary>
		/// This method is called when this fragment is seleted.
		/// </summary>
		public override void OnSelected()
		{
			if (Layout != null)
			{
				// Show spinner
				Layout.RemoveAllViews();
				Layout.AddView(new ProgressBar(Activity));

				ThreadPool.QueueUserWorkItem(o =>
				{
					// Get events
					var events = MainActivity.CIF.GetJoinedEvents(100);

					Activity.RunOnUiThread(() =>
					{
						// Set Adapters items
						Adapter.SetItems(events);

						// Remove spinner, and add listview
						Layout.RemoveAllViews();
						Layout.AddView(List);
					});
				});
			}
		}
	}
}