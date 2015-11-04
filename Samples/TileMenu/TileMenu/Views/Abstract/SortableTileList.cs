
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

		public SortableTileList(Context context, BaseTileListAdapter<T> adabter, string[] spinneritems)
			: base(context, adabter)
		{
			SortSpinnerView = new Spinner(context);

			SortSpinnerView.SetBackgroundColor(new Android.Graphics.Color(255,255,255));
			SortSpinnerView.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.TestListItem, spinneritems);
			
			#region Spinner Setup
			// sort when new item is selected in spinner
			SortSpinnerView.ItemSelected += (sender, e) => 
				{
					ExpListView.CollapseGroup(Focus);
					Adapter.Sort((e.View as TextView).Text);

				};
			#endregion
		}

		protected override void Initialize()
		{
			var sortspinnerlayout = new RelativeLayout.LayoutParams(-1, -2);
			var explistlayout = new RelativeLayout.LayoutParams(-1, -2);
			SortSpinnerView.Id = 10;

			AddView(SortSpinnerView);
			AddView(ExpListView);

			sortspinnerlayout.AddRule(LayoutRules.AlignParentBottom);
			SortSpinnerView.LayoutParameters = sortspinnerlayout;

			explistlayout.AddRule(LayoutRules.Above, SortSpinnerView.Id);
			ExpListView.LayoutParameters = explistlayout;

		}
	}
}

