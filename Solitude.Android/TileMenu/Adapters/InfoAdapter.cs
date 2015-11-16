/*
 * I'm a dumbass.
 * 			- Jimmi
 */


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

namespace DineWithaDane.Android
{
	public class InfoAdapter : BaseTileListAdapter<List<int>>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public InfoAdapter(Activity context, List<int>[] items) 
			: base(context, items.ToList()) { }
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
			var view = (convertView as InfoGroup); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new InfoGroup(Context);

			// set view information
			view.Title = MainActivity.InfoTitles[groupPosition];

			if (isExpanded)
			{
				view.SetArrowDirection(Direction.Up);
				if (Items[groupPosition].Count != 0)
					view.SetSeperatorVisibility(ViewStates.Gone);
			}
			else
			{
				view.SetArrowDirection(Direction.Down);
				view.SetSeperatorVisibility(ViewStates.Visible);
			}

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
			var view = (convertView as InfoItem); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new InfoItem(Context);

			// set view information
			view.Descrition = MainActivity.InfoNames[groupPosition][Items[groupPosition][childPosition]];

			if (Items[groupPosition].Count - 1 == childPosition)
				view.SetSeperatorVisibility(ViewStates.Visible);
			else
				view.SetSeperatorVisibility(ViewStates.Invisible);
				

			return view;
		}

		/// <summary>
		/// Sort the specified context.
		/// </summary>
		/// <param name="context">The way in which the items should be sorted.</param>
		public override void Sort(string context)
		{
		}

		/// <param name="groupPosition">the position of the group for which the children
		///  count should be returned</param>
		/// <summary>
		/// Gets the number of children in a specified group.
		/// </summary>
		public override int GetChildrenCount(int groupPosition)
		{
			return Items[groupPosition].Count;
		}
		#endregion


		#region Private Methods
		#endregion
	}
}

