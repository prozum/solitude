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
	public class EventGroup : TileListGroup
	{
		public string Title
		{
			set
			{
				TitleView.Text = "Title: " + value;
			}
		}

		public string Place
		{
			set
			{
				PlaceView.Text = "Place: " + value;
			}
		}

		public DateTime Date
		{
			set
			{
				DateView.Text = "Date: " + value.ToString();
			}
		}

		protected TextView TitleView
		{
			get;
			set;
		}

		protected TextView PlaceView
		{
			get;
			set;
		}

		protected TextView DateView
		{
			get;
			set;
		}

		public EventGroup(Context context)
			: base(context)
		{
			TitleView = new TextView(context);
			PlaceView = new TextView(context);
			DateView = new TextView(context);

			Initialize();
		}

		protected override void Initialize()
		{
			var detailslayout = new LinearLayout(Context);
			detailslayout.Orientation = Orientation.Horizontal;

			AddView(TitleView);
			AddView(detailslayout);
			detailslayout.AddView(PlaceView);
			detailslayout.AddView(DateView);

			detailslayout.WeightSum = 2;
			TitleView.LayoutParameters.Width = -1;
			PlaceView.LayoutParameters = new LinearLayout.LayoutParams(0, -2, 1);
			DateView.LayoutParameters = new LinearLayout.LayoutParams(0, -2, 1);

			base.Initialize();
		}
	}
}

