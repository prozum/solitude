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

namespace Solitude.Droid
{
	public class RecommendsFragment : Android.Support.V4.App.Fragment
	{
		protected List<Offer> Offers { get; set; }

		public RecommendsFragment(List<Offer> offers)
		{
			Offers = offers;
        }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
			
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			var layout = inflater.Inflate(Resource.Layout.EventList, container, false);
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var adapter = new EventAdapter<Offer>(Activity, Offers);

			adapter.OnAction1 = (i) => adapter.RemoveAt(i);
			adapter.OnAction2 = (i) => adapter.RemoveAt(i);
			adapter.OnUpdatePosition = (view, evnt, exp) =>
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

			list.Adapter = adapter;

			return layout;
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