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
using Android.Graphics;

namespace DineWithaDane.Android
{
	public abstract class SortableTileList<T> : TileList<T>
	{
		#region Fields
		protected Spinner SortSpinnerView {	get; set; }
		#endregion


		#region Constructors
		public SortableTileList(Context context, BaseTileListAdapter<T> adabter, string[] spinneritems)
			: base(context, adabter)
		{
			SortSpinnerView = new Spinner(context);

			SortSpinnerView.SetBackgroundColor(new Color(255,255,255));
			SortSpinnerView.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SpinnerItem,spinneritems);

			#region Spinner Setup
			// sort when new item is selected in spinner
			SortSpinnerView.ItemSelected += (sender, e) => 
				{
					ExpListView.CollapseGroup(Focus);

					if (e.View != null)
						Adapter.Sort((e.View as TextView).Text);
				};
			#endregion
		}
		#endregion


		#region Private Methods
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
		#endregion
	}
}