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
	public class RecommendsFragment : TabFragment
	{
		public FrameLayout Layout { get; set; }
		public ListView List { get; set; }
		public EventAdapter<Offer> Adapter { get; set; }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.EventList, container, false)
							 .FindViewById<FrameLayout>(Resource.Id.layout);
			List = Layout.FindViewById<ListView>(Resource.Id.list);
			Layout.RemoveAllViews();
			Layout.AddView(new ProgressBar(Activity));

			ThreadPool.QueueUserWorkItem(o =>
			{
				Adapter = new EventAdapter<Offer>(Activity, MainActivity.CIF.RequestOffers());

				Adapter.OnAction1 = (i) =>
				{
					var @event = Adapter.Items[i];
					MainActivity.CIF.ReplyOffer(false, @event);
					Adapter.RemoveAt(i);
					AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_decline) + @event.Title, 2000).Show();
				};
				Adapter.OnAction2 = (i) =>
				{
					var @event = Adapter.Items[i];
					MainActivity.CIF.ReplyOffer(true, @event);
					Adapter.RemoveAt(i);
					AccentSnackBar.Make(Layout, Activity, Resources.GetString(Resource.String.event_accepted) + @event.Title, 2000).Show();
				};
				Adapter.OnUpdatePosition = (view, evnt, exp) =>
				{
					var offer = evnt as Offer;
					string matchs = GetString(Resource.String.event_matchedby) + ":\n";

					AddInfo(offer.Match.Interests, InfoType.Interest, matchs);
					AddInfo(offer.Match.FoodHabits, InfoType.FoodHabit, matchs);
					AddInfo(offer.Match.Languages, InfoType.Language, matchs);

					view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
						string.Format("{0}\n\n{1}: {2}\n{3}: {4}/{5}\n\n{6}", 
									  offer.Description, Resources.GetString(Resource.String.event_place), 
									  offer.Address, Resources.GetString(Resource.String.event_participants), 
									  offer.SlotsTaken, offer.SlotsTotal, matchs);

					view.FindViewById<Button>(Resource.Id.action1).Text = GetString(Resource.String.decline_button);
					view.FindViewById<Button>(Resource.Id.action2).Text = GetString(Resource.String.accept_button);
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
					var offers = MainActivity.CIF.RequestOffers();

					Activity.RunOnUiThread(() =>
					{
						Adapter.SetItems(offers);

						Layout.RemoveAllViews();
						Layout.AddView(List);
					});
				});
			}
		}

		private void AddInfo(int[] items, InfoType type, string res)
		{
			for (int i = 0; i < items.Length; i++)
			{
				res += MainActivity.InfoNames[(int)type][items[i]];

				if (i != items.Length - 1)
					res += ", ";
				else
					res += "\n";
			}
		}
	}
}