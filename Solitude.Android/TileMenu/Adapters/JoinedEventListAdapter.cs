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
	public class JoinedEventListAdapter : BaseTileListAdapter<Event>
	{
		#region Fields
		/// <summary>
		/// Gets or sets the handler for the cancel button of the views.
		/// </summary>
		public EventHandler OnLeave { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public JoinedEventListAdapter(Activity context, List<Event> items) 
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
			var view = (convertView as JoinedEventItem); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new JoinedEventItem(Context, OnLeave);

			// set view information
			view.Descrition = Items[groupPosition].Description;

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