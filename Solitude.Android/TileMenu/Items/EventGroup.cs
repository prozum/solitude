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
		#region Field
		public string Title { set { TitleView.Text = "Title: " + value; } }
		public string Place	{ set { PlaceView.Text = "Place: " + value;	} }
		public DateTime Date { set { DateView.Text = "Date: " + value; } }
		public Tuple<int,int> Slots 
		{
			set 
			{ 
				ParticipantsView.Text = "Participants: " + (value.Item2 - value.Item1) + "/" + value.Item2; 
			} 
		}

		protected TextView TitleView { get; set; }
		protected TextView PlaceView { get; set; }
		protected TextView DateView { get; set; }
		protected TextView ParticipantsView { get; set; }
		#endregion


		#region Contructors
		public EventGroup(Context context)
			: base(context)
		{
			TitleView = new TextView(context);
			PlaceView = new TextView(context);
			DateView = new TextView(context);
			ParticipantsView = new TextView(context);

			Initialize();
		}
		#endregion


		#region Private Methods
		protected override void Initialize()
		{
			var detailslayout = new LinearLayout(Context);
			detailslayout.Orientation = Orientation.Horizontal;

			AddView(TitleView);
			AddView(detailslayout);
			AddView(ParticipantsView);
			detailslayout.AddView(PlaceView);
			detailslayout.AddView(DateView);

			detailslayout.WeightSum = 2;
			TitleView.LayoutParameters.Width = -1;
			ParticipantsView.LayoutParameters.Width = -1;
			PlaceView.LayoutParameters = new LinearLayout.LayoutParams(0, -2, 1);
			DateView.LayoutParameters = new LinearLayout.LayoutParams(0, -2, 1);

			base.Initialize();
		}
		#endregion
	}
}