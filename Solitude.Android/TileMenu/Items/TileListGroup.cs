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
	public abstract class TileListGroup : LinearLayout
	{
		#region Fields
		protected TextView DotsView { get; set;	}
		protected TextView SeperatorView { get; set; }
		#endregion


		#region Constructors
		public TileListGroup(Context context)
			: base(context)
		{
			DotsView = new TextView(context);
			SeperatorView = new Seperator(context);

			Orientation = Orientation.Vertical;

			DotsView.Text = "...";
			SeperatorView.SetBackgroundColor(new Color(255,255,255));

			DotsView.Gravity = GravityFlags.Center;
		}
		#endregion


		#region Public Methods
		public void SeperatorVisibility(ViewStates state)
		{
			DotsView.Visibility = state;
			SeperatorView.Visibility = state;
		}
		#endregion


		#region Private Methods
		protected virtual void Initialize()
		{
			AddView(DotsView);
			AddView(SeperatorView);

			DotsView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Height = 1;
		}
		#endregion




	}
}