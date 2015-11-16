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
	public abstract class BaseTileListAdapter<T> : BaseExpandableListAdapter
	{
		#region Fields
		protected Activity Context { get; private set; }

		/// <summary>
		/// The values the adapter will populate views with.
		/// </summary>
		public List<T> Items { get; private set; }

		/// <summary>
		/// Total number of items in adapter.
		/// </summary>
		public override int GroupCount { get { return Items.Count; } }

		/// <summary>
		/// Indicates whether the child and group IDs are stable across changes to the
		///  underlying data.
		/// </summary>
		public override bool HasStableIds {	get { return true; } } 
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.BaseTileListAdapter`1"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public BaseTileListAdapter(Activity context, List<T> items) 
			: base()
		{
			if (items == null)
				throw new NullReferenceException("items was null");

			Context = context;
			Items = items;
		}
		#endregion


		#region Public Methods
		/// <summary>
		/// Sort the specified context.
		/// </summary>
		/// <param name="context">The way in which the items should be sorted.</param>
		public abstract void Sort(string context);

		/// <summary>
		/// Removes at index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveAt(int index)
		{
			Items.RemoveAt(index);
			NotifyDataSetChanged();
		}

		/// <param name="groupPosition">the position of the group for which the ID is wanted</param>
		/// <summary>
		/// Gets the ID for the group at the given position.
		/// </summary>
		public override long GetGroupId(int groupPosition)
		{
			return groupPosition;
		}

		/// <param name="groupPosition">the position of the group that contains the child</param>
		/// <param name="childPosition">the position of the child within the group for which
		///  the ID is wanted</param>
		/// <summary>
		/// Gets the ID for the given child within the given group.
		/// </summary>
		public override long GetChildId(int groupPosition, int childPosition)
		{
			return groupPosition;
		}

		/// <param name="groupPosition">the position of the group for which the children
		///  count should be returned</param>
		/// <summary>
		/// Gets the number of children in a specified group.
		/// </summary>
		public override int GetChildrenCount(int groupPosition)
		{
			return 1;
		}

		/// <param name="groupPosition">the position of the group that contains the child</param>
		/// <param name="childPosition">the position of the child within the group</param>
		/// <summary>
		/// Whether the child at the specified position is selectable.
		/// </summary>
		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return false;
		}

		/// <summary>
		/// Don't call this method.
		/// </summary>
		public override Java.Lang.Object GetGroup(int groupPosition)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Don't call this method.
		/// </summary>
		public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}