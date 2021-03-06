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
	public class RecommendsFragment : TabFragment
	{
		public FrameLayout Layout { get; set; }
		public ListView List { get; set; }
		public EventAdapter<Offer> Adapter { get; set; }

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
				// Get recommended events and create adapter.
				Adapter = new EventAdapter<Offer>(Activity, MainActivity.CIF.RequestOffers());
				
				// On decline click
				Adapter.OnAction1 = (i) =>
				{
					// get event
					var @event = Adapter.Items[i];

					// reply and remove event
					MainActivity.CIF.ReplyOffer(false, @event);
					Adapter.RemoveAt(i);

					// Show update snackbar
					AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_decline) + @event.Title, 2000).Show();
				};
				// On accept click
				Adapter.OnAction2 = (i) =>
				{
					// get event
					var @event = Adapter.Items[i];

					// reply and remove event
					MainActivity.CIF.ReplyOffer(true, @event);
					Adapter.RemoveAt(i);

					// Show update snackbar
					AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_accepted) + @event.Title, 2000).Show();
				};
				Adapter.OnUpdatePosition = (view, evnt, exp) =>
				{
					var offer = evnt as Offer;
					string matchs = GetString(Resource.String.event_matchedby) + ":\n";
					
					view.FindViewById<TextView>(Resource.Id.title).Text = offer.Title;
					view.FindViewById<TextView>(Resource.Id.subtitle).Text = offer.Date.ToString("G");

					AddInfo(offer.Match.Interests, InfoType.Interest, ref matchs);
					AddInfo(offer.Match.FoodHabits, InfoType.FoodHabit, ref matchs);
					AddInfo(offer.Match.Languages, InfoType.Language, ref matchs);

					// Content of card contains description, location, slotstaken, slotstotal and matches.
					view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
						string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}\n\n{6}", 
									  offer.Description, Resources.GetString(Resource.String.event_place), 
									  offer.Location, Resources.GetString(Resource.String.event_participants), 
									  offer.SlotsTaken, offer.SlotsTotal, matchs);

					view.FindViewById<Button>(Resource.Id.action1).Text = GetString(Resource.String.decline_button);
					view.FindViewById<Button>(Resource.Id.action2).Text = GetString(Resource.String.accept_button);
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
					var offers = MainActivity.CIF.RequestOffers();

					Activity.RunOnUiThread(() =>
					{
						// Set listview adapters items
						Adapter.SetItems(offers);
						
						// Remove spinner, and add listview
						Layout.RemoveAllViews();
						Layout.AddView(List);
					});
				});
			}
		}

		/// <summary>
		/// A method for adding the matched by information to a string
		/// </summary>
		private void AddInfo(int[] items, InfoType type, ref string res)
		{
			for (int i = 0; i < items.Length; i++)
			{
				var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)type, 0);
				res += Resources.GetStringArray(id)[items[i]];

				if (i != items.Length - 1)
					res += ", ";
				else
					res += "\n";
			}
		}
	}
}