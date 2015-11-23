using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;

namespace DineWithaDane.Android
{
	public abstract class BaseCardAdapter<T> : BaseAdapter
	{
		protected Activity Context { get; set; }
		protected List<T> Items { get; set; }

		public override int Count { get { return Items.Count; } }

		public BaseCardAdapter(Activity context, List<T> items)
		{
			Context = context;
			Items = items;
		}

		public override Java.Lang.Object GetItem(int position)
		{
			throw new NotImplementedException();
		}

		public override long GetItemId(int position)
		{
			return position;
		}


	}
}

