using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Solitude.Droid
{
	/// <summary>
	/// The adapter for handeling event cards.
	/// </summary>
	public class EventAdapter<T> : BaseAdapter where T : Event
	{
		#region Fields
		/// <summary>
		/// The action called, when the card changes postion in the ListView.
		/// This action should specify how the view should be drawn.
		/// </summary>
		public Action<View, Event, bool> OnUpdatePosition { get; set; }

		/// <summary>
		/// The action called, when button one is pressed.
		/// </summary>
		public Action<int> OnAction1 { get; set; }

		/// <summary>
		/// The action called, when button two is pressed.
		/// </summary>
		public Action<int> OnAction2 { get; set; }

		/// <summary>
		/// The items in the adapter.
		/// </summary>
		public List<T> Items { get; private set; }

		/// <summary>
		/// The total number of items in the adapter.
		/// </summary>
		public override int Count { get { return Items.Count; } }
		public override bool HasStableIds { get { return true; } }

		/// <summary>
		/// The activity this adapter is contained in.
		/// </summary>
		protected Activity Context { get; set; }

		/// <summary>
		/// A list for keeping track of what cards are expanded.
		/// </summary>
		protected List<bool> IsExpanded { get; set; }
		#endregion


		#region Constructors
		public EventAdapter(Activity context, List<T> items)
			: base()
		{
			if (items == null)
				throw new ArgumentNullException();

			Context = context;
			Items = items;
			IsExpanded = new List<bool>();

			// Add items to IsExpanded, untill IsExpanded.Count is equal to Items.Count
			for (int i = 0; i < Items.Count; i++)
				IsExpanded.Add(false);
		}
		#endregion


		#region Public Methods
		/// <summary>
		/// Set the Items list to a new list of Items.
		/// </summary>
		public void SetItems(List<T> items)
		{
			Items = items;
			IsExpanded = new List<bool>();

			// Add items to IsExpanded, untill IsExpanded.Count is equal to Items.Count
			for (int i = 0; i < Items.Count; i++)
				IsExpanded.Add(false);

			// Notify the ListView, that the dataset changed.
			NotifyDataSetChanged();
		}

		/// <summary>
		/// Get the view of an Item
		/// </summary>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = (convertView as EventItem<T>);

			// If no view was found, make a new one.
			if (view == null)
				view = new EventItem<T>(Context, this, OnExpand, OnUpdatePosition, OnAction1, OnAction2);

			// Update the views data, to so that the data matches its position.
			view.UpdatePosition(position, IsExpanded[position]);

			return view;
		}

		/// <summary>
		/// Removes an items on at an index.
		/// </summary>
		public void RemoveAt(int index)
		{
			Items.RemoveAt(index);
			IsExpanded.RemoveAt(index);

			// Notify the ListView, that the dataset changed.
			NotifyDataSetChanged();
		}

		/// <summary>
		/// Get an items Id
		/// </summary>
		public override long GetItemId(int position)
		{
			return position;
		}

		/// <summary>
		/// This method is never called
		/// </summary>
		public override Java.Lang.Object GetItem(int position)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// The method called, when a view is expanded
		/// </summary>
		protected void OnExpand(int pos)
		{
			IsExpanded[pos] = !IsExpanded[pos];

			// Notify the ListView, that the dataset changed.
			NotifyDataSetChanged();
		}
		#endregion
	}
}