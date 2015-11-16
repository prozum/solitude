using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Android
{
	public class OfferListAdapter : BaseTileListAdapter<Offer>
	{
		#region Fields
		/// <summary>
		/// Gets or sets the handler for the accept button of the views.
		/// </summary>
		public EventHandler OnAccept { get; set; }

		/// <summary>
		/// Gets or sets the handler for the decline button of the views.
		/// </summary>
		public EventHandler OnDecline { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.OfferListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public OfferListAdapter(Activity context, List<Offer> items) 
			: base(context, items) { }
		#endregion


		#region Public Methods
		/// <param name="groupPosition">the position of the group for which the View is
		///  returned</param>
		/// <param name="isExpanded">whether the group is expanded or collapsed</param>
		/// <summary>
		/// Gets the group view.
		/// </summary>
		/// <returns>The group view.</returns>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			var view = (convertView as EventGroup); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new EventGroup(Context);

			// set view information
			view.Title = Items[groupPosition].Title;
			view.Place = Items[groupPosition].Address;
			view.Date = Items[groupPosition].Date;
			view.Slots = new Tuple<int, int>(Items[groupPosition].SlotsLeft, Items[groupPosition].SlotsTotal);

			// set seperator visibility
			if (isExpanded)
				view.SetSeperatorVisibility(ViewStates.Gone);
			else
				view.SetSeperatorVisibility(ViewStates.Visible);

			return view;
		}

		/// <param name="groupPosition">the position of the group that contains the child</param>
		/// <param name="childPosition">the position of the child (for which the View is
		///  returned) within the group</param>
		/// <param name="isLastChild">Whether the child is the last child within the group</param>
		/// <summary>
		/// Gets the child view.
		/// </summary>
		/// <returns>The child view.</returns>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			var view = (convertView as OfferItem); // re-use an existing view, if one is available

			if (view == null)// otherwise create a new one
				view = new OfferItem(Context, OnAccept, OnDecline);

			var item = Items[groupPosition];

			// set view information
			view.Descrition = item.Description;
			view.MatchedBy = GetMatchedByString(item);
			//view.MatchedBy = Items[groupPosition]

			return view;
		}

		/// <summary>
		/// Sort the specified context.
		/// </summary>
		/// <param name="context">The way in which the items should be sorted.</param>
		public override void Sort(string context)
		{
			switch (context)
			{
				case"Title (A-Z)":
					Items.Sort(CompareTitle);
					break;
				case"Title (Z-A)":
					Items.Sort(CompareTitle);
					Items.Reverse();
					break;
				case"Date (Soonest)":
					Items.Sort(CompareDate);
					break;
				case"Date (Last)":
					Items.Sort(CompareDate);
					Items.Reverse();
					break;
				case"Distance (Closest)":
					Items.Sort(CompareDistance);
					break;
				case"Distance (Farthest)":
					Items.Sort(CompareDistance);
					Items.Reverse();
					break;
				default:
					throw new NotImplementedException();

			}

			NotifyDataSetChanged();
		}
		#endregion


		#region Private Methods
		private string GetMatchedByString(Offer item)
		{
			var res = "";
			var interestlast = item.Match.Interests.Count() - 1;
			var languagelast = item.Match.Languages.Count() - 1;
			var foodhabitlast = item.Match.FoodHabits.Count() - 1;

			for (int i = 0; i < interestlast; i++)
				res += InfoAdapter.Names[1][item.Match.Interests[i]] + ", ";

			if (interestlast >= 0)
				res += InfoAdapter.Names[1][item.Match.Interests[interestlast]] + "\n";
			
			for (int i = 0; i < languagelast; i++)
				res += InfoAdapter.Names[0][item.Match.Languages[i]] + ", ";

			if (languagelast >= 0)
				res += InfoAdapter.Names[0][item.Match.Languages[languagelast]] + "\n";

			for (int i = 0; i < foodhabitlast; i++)
				res += InfoAdapter.Names[2][item.Match.FoodHabits[i]] + ", ";

			if (foodhabitlast >= 0)
				res += InfoAdapter.Names[2][item.Match.FoodHabits[foodhabitlast]] + "\n";

			return res;
		}

		private int CompareTitle(Event x, Event y)
		{
			return x.Title.CompareTo(y.Title);
		}

		private int CompareDate(Event x, Event y)
		{
			return x.Date.CompareTo(y.Date);
		}

		private int CompareDistance(Event x, Event y)
		{
			return x.Address.CompareTo(y.Address);
		}
		#endregion
	}
}