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
		public string Descrition
		{
			set
			{
				DescritionView.Text = "Description: " + value;
			}
		}

		protected TextView DescritionView
		{
			get;
			set;
		}

		protected TextView SeperatorView
		{
			get;
			set;
		}

		public TileListItem(Context context)
			: base(context)
		{
			Orientation = Orientation.Vertical;
			SeperatorView = new TextView(context);

			SeperatorView.SetBackgroundColor(new Color(255,255,255));

			DescritionView = new TextView(context);
		}

		protected virtual void Initialize()
		{
			AddView(SeperatorView);

			DescritionView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Height = 1;
		}
	}
}

