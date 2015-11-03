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
	public class EventGroup : TileListGroup
	{
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

		protected LinearLayout MoreDetailsView
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
			MoreDetailsView = new LinearLayout(context);
			PlaceView = new TextView(context);
			DateView = new TextView(context);

			MoreDetailsView.Orientation = Orientation.Horizontal;

			Initialize();
		}

		public void SeperatorVisibility(ViewStates state)
		{
			DotsView.Visibility = state;
			SeperatorView.Visibility = state;
		}

		protected override void Initialize()
		{
			AddView(TitleView);
			AddView(MoreDetailsView);
			AddView(DotsView);
			AddView(SeperatorView);
			MoreDetailsView.AddView(PlaceView);
			MoreDetailsView.AddView(DateView);

			MoreDetailsView.WeightSum = 2;
			TitleView.LayoutParameters.Width = -1;
			DotsView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Width = -1;
			SeperatorView.LayoutParameters.Height = 1;
			PlaceView.LayoutParameters = new LinearLayout.LayoutParams(0, -2, 1);
			DateView.LayoutParameters = new LinearLayout.LayoutParams(0, -2, 1);

		}
	}
}

