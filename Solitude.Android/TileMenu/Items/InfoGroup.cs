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
	public class InfoGroup : TileListGroup
	{
		#region Field
		/// <summary>
		/// Sets the title of the view.
		/// </summary>
		public string Title { set { TitleView.Text = value; } }

		protected TextView TitleView { get; set; }
		#endregion


		#region Contructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventGroup"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public InfoGroup(Context context)
			: base(context)
		{
			TitleView = new TextView(context);

			TitleView.SetTypeface(null, TypefaceStyle.Bold);

			Initialize();
		}
		#endregion


		#region Private Methods
		protected override void Initialize()
		{
			AddView(TitleView);
			AddView(SeperatorView);

			TitleView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Width = -1;

			TitleView.TextSize = 20;
			TitleView.SetPadding(0, 8, 0, 8);
			TitleView.SetTypeface(null, TypefaceStyle.Bold);

		}
		#endregion
	}
}