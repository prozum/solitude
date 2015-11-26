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
				string matchs = "Matched by:\n";

				for (int i = 0; i < offer.Match.Interests.Length; i++)
				{
					matchs += MainActivity.InfoNames[(int)InfoType.Interest][offer.Match.Interests[i]];

					if (i != offer.Match.Interests.Length - 1)
						matchs += ", ";
					else
						matchs += "\n";
				}


				for (int i = 0; i < offer.Match.FoodHabits.Length; i++)
				{
					matchs += MainActivity.InfoNames[(int)InfoType.FoodHabit][offer.Match.FoodHabits[i]];

					if (i != offer.Match.FoodHabits.Length - 1)
						matchs += ", ";
					else
						matchs += "\n";
				}

				for (int i = 0; i < offer.Match.FoodHabits.Length; i++)
				{
					matchs += MainActivity.InfoNames[(int)InfoType.Language][offer.Match.Languages[i]];

					if (i != offer.Match.Languages.Length - 1)
						matchs += ", ";
					else
						matchs += "\n";
				}

				view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
					string.Format("{0}\n\n{1}\n{2}/{3}\n{4}", offer.Description, offer.Address, offer.SlotsTaken, offer.SlotsTotal, matchs);

				view.FindViewById<Button>(Resource.Id.action1).Text = "Delcine";
				view.FindViewById<Button>(Resource.Id.action2).Text = "Accept";
			};

			list.Adapter = adapter;

			return layout;
			//return base.OnCreateView(inflater, container, savedInstanceState);
		}
	}
}