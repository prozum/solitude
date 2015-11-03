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
	public abstract class TileListGroup : LinearLayout
	{
		public string Title
		{
			set
			{
				TitleView.Text = "Title: " + value;
			}
		}

		protected TextView TitleView
		{
			get;
			set;
		}

		protected TextView DotsView
		{
			get;
			set;
		}

		protected TextView SeperatorView
		{
			get;
			set;
		}

		public TileListGroup(Context context)
			: base(context)
		{
			TitleView = new TextView(context);
			DotsView = new TextView(context);
			SeperatorView = new TextView(context);

			Orientation = Orientation.Vertical;

			DotsView.Text = "...";
			SeperatorView.SetBackgroundColor(new Android.Graphics.Color(255,255,255));

			DotsView.Gravity = GravityFlags.Center;
		}

		protected virtual void Initialize()
		{
			AddView(TitleView);
			AddView(DotsView);
			AddView(SeperatorView);

			TitleView.LayoutParameters.Width = -1;
			DotsView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Height = 1;
		}
	}
}

