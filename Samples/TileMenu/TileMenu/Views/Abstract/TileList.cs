
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

namespace TileMenu
{
	public abstract class TileList<T> : LinearLayout
	{
		protected ExpandableListView ExpListView
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
			ExpListView = new ExpandableListView(Context);
			ExpListView.SetAdapter(adabter);
			ExpListView.SetGroupIndicator(null);

			// focus selfcollapse, when another item expands
			ExpListView.GroupExpand += (sender, e) => 
				{
					if (Focus != e.GroupPosition) {
						ExpListView.CollapseGroup(Focus);
						Focus = e.GroupPosition;
					}
				};
		}

		protected virtual void Initialize()
		{
			AddView(ExpListView);

			ExpListView.LayoutParameters.Width = -1;
		}

		protected virtual void OnButton1(object sender, EventArgs e)
		{
			(ExpListView.ExpandableListAdapter as BaseTileListAdapter<T>).RemoveAt(Focus);
			ExpListView.CollapseGroup(Focus);
		}

		protected virtual void OnButton2(object sender, EventArgs e)
		{
			(ExpListView.ExpandableListAdapter as BaseTileListAdapter<T>).RemoveAt(Focus);
			ExpListView.CollapseGroup(Focus);
		}
	}
}

