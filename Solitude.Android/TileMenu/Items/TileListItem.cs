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
	public abstract class TileListItem : LinearLayout
	{
		#region Fields
		/// <summary>
		/// Sets the descrition of the view.
		/// </summary>
		/// <value>The descrition.</value>
		public string Descrition { set { DescritionView.Text = "Description: " + value; } }

		protected TextView DescritionView {	get; set; }
		protected TextView SeperatorView { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.TileListItem"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public TileListItem(Context context)
			: base(context)
		{
			Orientation = Orientation.Vertical;
			SeperatorView = new Seperator(context);
			DescritionView = new TextView(context);

			SeperatorView.SetBackgroundColor(new Color(255,255,255));
		}
		#endregion


		#region Private Methods
		protected virtual void Initialize()
		{
			AddView(SeperatorView);

			DescritionView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Width = -1;
		}
		#endregion
	}
}