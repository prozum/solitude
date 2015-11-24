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

namespace DineWithaDane.Droid
{
	public abstract class SortableTileList<T> : TileList<T>
	{
		#region Fields
		public static readonly string[] SortItems = new string[] 
			{
				"Title (A-Z)",
				"Title (Z-A)",
				"Date (Soonest)",
				"Date (Last)",
				"Distance (Closest)",
				"Distance (Farthest)"
			};

		protected Spinner SortSpinnerView {	get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.SortableTileList`1"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adabter">Adabter.</param>
		/// <param name="spinneritems">Spinneritems.</param>
		public SortableTileList(Context context, BaseTileListAdapter<T> adabter, string[] spinneritems)
			: base(context, adabter)
		{
			SortSpinnerView = new Spinner(context);

			//SortSpinnerView.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SpinnerItem,spinneritems);

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
			var seperator = new Seperator(Context);
			var seperatorparams = new RelativeLayout.LayoutParams(-1, -2);
			var sortspinnerparams = new RelativeLayout.LayoutParams(-1, -2);
			var explistparams = new RelativeLayout.LayoutParams(-1, -2);
			SortSpinnerView.Id = 10;
			seperator.Id = 11;

			AddView(seperator);
			AddView(SortSpinnerView);
			AddView(ExpListView);

			sortspinnerparams.AddRule(LayoutRules.AlignParentBottom);
			seperatorparams.AddRule(LayoutRules.Above, SortSpinnerView.Id);
			explistparams.AddRule(LayoutRules.Above, seperator.Id);
			explistparams.AddRule(LayoutRules.AlignParentTop);

			seperator.LayoutParameters = seperatorparams;
			SortSpinnerView.LayoutParameters = sortspinnerparams;
			ExpListView.LayoutParameters = explistparams;

		}
		#endregion
	}
}