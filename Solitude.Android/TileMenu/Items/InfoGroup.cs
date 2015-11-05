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

namespace DineWithaDane.Android
{
	public class InfoGroup : TileListGroup
	{
		public string InfoName
		{
			set
			{
				InfoNameView.Text = value;
			}
		}

		protected TextView InfoNameView
		{
			get;
			set;
		}

		public InfoGroup(Context context)
			: base(context)
		{
			InfoNameView = new TextView(context);

			Initialize();
		}

		protected override void Initialize()
		{
			AddView(InfoNameView);

			InfoNameView.LayoutParameters.Width = -1;

			base.Initialize();
		}
	}
}

