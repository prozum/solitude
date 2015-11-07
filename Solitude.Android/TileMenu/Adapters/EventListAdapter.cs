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
	public class EventListAdapter : BaseTileListAdapter<Event>
	{
		#region Fields
		public EventHandler OnCancel { get; set; }
		#endregion


		#region Constructors
		public EventListAdapter(Activity context, List<Event> items) 
			: base(context, items) { }
		#endregion


		#region Public Methods
		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			var view = (convertView as EventGroup); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new EventGroup(Context);

			// set view information
			view.Title = Items[groupPosition].Title;
			view.Place = Items[groupPosition].Place;
			view.Date = Items[groupPosition].Date;
			view.Slots = new Tuple<int, int>(Items[groupPosition].SlotsLeft, Items[groupPosition].MaxSlots);

			// set seperator visibility
			if (isExpanded)
				view.SeperatorVisibility(ViewStates.Gone);
			else
				view.SeperatorVisibility(ViewStates.Visible);

			return view;
		}

		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			var view = (convertView as EventItem); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new EventItem(Context, OnCancel);

			// set view information
			view.Descrition = Items[groupPosition].Description;

			return view;
		}

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
				case"Date (Lastest)":
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
			return x.Place.CompareTo(y.Place);
		}
		#endregion
	}
}