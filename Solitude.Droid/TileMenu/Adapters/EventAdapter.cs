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
	public class EventAdapter<T> : BaseAdapter where T : Event
	{
		#region Fields
		public Action<View, Event, bool> OnUpdatePosition { get; set; }
		public Action<int> OnAction1 { get; set; }
		public Action<int> OnAction2 { get; set; }

		public List<T> Items { get; private set; }
		public override int Count { get { return Items.Count; } }
		public override bool HasStableIds { get { return true; } }

		protected Activity Context { get; set; }
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

			for (int i = 0; i < Items.Count; i++)
				IsExpanded.Add(false);
		}
		#endregion


		#region Public Methods
		public void SetItems(List<T> items)
		{
			Items = items;
			IsExpanded = new List<bool>();

			for (int i = 0; i < Items.Count; i++)
				IsExpanded.Add(false);

			NotifyDataSetChanged();
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = (convertView as EventItem<T>);

			if (view == null)
				view = new EventItem<T>(Context, this, OnExpand, OnUpdatePosition, OnAction1, OnAction2);

			view.UpdatePosition(position, IsExpanded[position]);

			return view;
		}


		public void RemoveAt(int index)
		{
			Items.RemoveAt(index);
			IsExpanded.RemoveAt(index);
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

		#region Private Methods
		protected void OnExpand(int pos)
		{
			IsExpanded[pos] = !IsExpanded[pos];
			NotifyDataSetChanged();
		}
		#endregion
	}
}