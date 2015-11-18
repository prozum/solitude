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
	public class EventGroup : TileListGroup
	{
		#region Field

		/// <summary>
		/// Sets the title of the view.
		/// </summary>
		public string Title { set { TitleView.Text = /*"Title: " +*/ value; } }

		/// <summary>
		/// Sets the place of the view.
		/// </summary>
		public string Place	{ set { PlaceView.Text = /*"Place: " +*/ value; } }

		/// <summary>
		/// Sets the date of the view.
		/// </summary>
		public DateTimeOffset Date { set { DateView.Text = /*"Date: " +*/ value.ToString("dd/MM/yyyy - hh:mm"); } }

		/// <summary>
		/// Sets the slots taken and total stols of the view. Item1 is slots taken. Item2 is totals slots.
		/// </summary>
		public Tuple<int,int> Slots
		{
			set
			{ 
				ParticipantsView.Text = "Participants: " + (value.Item1) + "/" + value.Item2; 
			} 
		}

		protected TextView TitleView { get; set; }

		protected TextView PlaceView { get; set; }

		protected TextView DateView { get; set; }

		protected TextView ParticipantsView { get; set; }

		#endregion


		#region Contructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventGroup"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public EventGroup(Context context)
			: base(context)
		{
			TitleView = new TextView(context);
			PlaceView = new TextView(context);
			DateView = new TextView(context);
			ParticipantsView = new TextView(context);

			TitleView.SetTypeface(null, TypefaceStyle.Bold);
			DateView.SetTypeface(null, TypefaceStyle.Italic);

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
			detailslayout.AddView(PlaceView);
			detailslayout.AddView(DateView);
			AddView(ParticipantsView);

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