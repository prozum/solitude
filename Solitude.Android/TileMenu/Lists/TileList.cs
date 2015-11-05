
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
	public abstract class TileList<T> : RelativeLayout
	{
		protected ExpandableListView ExpListView
		{
			get;
			set;
		}

		protected BaseTileListAdapter<T> Adapter
		{
			get;
			set;
		}

		protected int Focus
		{
			get;
			set;
		}

		public TileList(Context context, BaseTileListAdapter<T> adabter)
			: base(context)
		{
			Adapter = adabter;

			ExpListView = new ExpandableListView(Context);
			ExpListView.SetAdapter(Adapter);
			ExpListView.SetGroupIndicator(null);

			// focus selfcollapse, when another item expands
			ExpListView.GroupExpand += (sender, e) => 
				{
					if (Focus != e.GroupPosition) 
					{
						ExpListView.CollapseGroup(Focus);
						Focus = e.GroupPosition;
					}
				};
		}

		protected void RemoveFocus()
		{
			Adapter.RemoveAt(Focus);
		}


		protected virtual void Initialize()
		{
			AddView(ExpListView);

			ExpListView.LayoutParameters.Width = -1;
		}
	}
}

