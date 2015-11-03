
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
	public abstract class SortableTileList<T> : TileList<T>
	{
		protected Spinner SortSpinnerView
		{
			get;
			set;
		}

		public SortableTileList(Context context, BaseTileListAdapter<T> adabter)
			: base(context, adabter)
		{
			SortSpinnerView = new Spinner(context);

			#region Spinner Setup
			// sort when new item is selected in spinner
			SortSpinnerView.ItemSelected += (sender, e) => 
				{
					ExpListView.CollapseGroup(Focus);
					(ExpListView.ExpandableListAdapter as BaseTileListAdapter<T>).Sort(((e as AdapterView.ItemSelectedEventArgs).View as TextView).Text);

				};
			#endregion
		}

		protected override void Initialize()
		{
			AddView(ExpListView);
			AddView(SortSpinnerView);

			SortSpinnerView.LayoutParameters.Width = -1;
			ExpListView.LayoutParameters.Width = -1;
		}
	}
}

