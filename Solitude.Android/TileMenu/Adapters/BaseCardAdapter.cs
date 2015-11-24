using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Droid
{
	public abstract class BaseCardAdapter<T> : BaseAdapter
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
		public override int Count { get { return Items.Count; } }

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
		public BaseCardAdapter(Activity context, List<T> items) 
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
		/// Removes at index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveAt(int index)
		{
			Items.RemoveAt(index);
			NotifyDataSetChanged();
		}

		public void Remove(T item)
		{
			Items.Remove(item);
			NotifyDataSetChanged();
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Java.Lang.Object GetItem(int position)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}