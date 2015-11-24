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
	public abstract class TileListGroup : LinearLayout
	{
		#region Fields
		protected TextView DotsView { get; set;	}
		protected TextView SeperatorView { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.TileListGroup"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public TileListGroup(Context context)
			: base(context)
		{
			DotsView = new TextView(context);
			SeperatorView = new Seperator(context);

			Orientation = Orientation.Vertical;

			DotsView.Text = "...";

			DotsView.Gravity = GravityFlags.Center;
		}
		#endregion


		#region Public Methods
		/// <summary>
		/// Sets the seperator visibility.
		/// </summary>
		/// <param name="state">State.</param>
		public void SetSeperatorVisibility(ViewStates state)
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
		}
		#endregion
	}
}