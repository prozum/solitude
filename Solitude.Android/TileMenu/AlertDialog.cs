using System;
using Android.App;
using Android.Widget;
using Android.Views;
using System.Collections.Generic;

namespace DineWithaDane.Droid
{
	public class EventBody : AlertDialog
	{
		public EventBody(Activity context, string title, string date, string place, string host, List<string> guests)
			: base(context)
		{
			TableLayout table = new TableLayout(context);
			table.Orientation = Orientation.Horizontal;

			TableRow rowTitle = new TableRow(context);
			rowTitle.WeightSum = 3f;

			TextView innerTitle = new TextView(context);
			innerTitle.Text = title;
			innerTitle.TextSize = 24;
			innerTitle.Gravity = GravityFlags.Center;
			innerTitle.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 3.0f);

			table.AddView(innerTitle);

			TableRow rowDate = new TableRow(context);
			rowDate.WeightSum = 3f;

			TextView dateText = new TextView(context);
			dateText.Text = "When";
			dateText.Gravity = GravityFlags.Left;
			dateText.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			dateText.SetPadding(50, 10, 50, 10);

			TextView dateDisp = new TextView(context);
			dateDisp.Text = "" + DateTime.Now.ToString() + "";
			dateDisp.Gravity = GravityFlags.Left;
			dateDisp.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 2.0f);
			dateDisp.SetPadding(50, 10, 50, 10);

			rowDate.AddView(dateText);
			rowDate.AddView(dateDisp);
			table.AddView(rowDate);

			TableRow rowPlace = new TableRow(context);
			rowPlace.WeightSum = 3f;

			TextView placeText = new TextView(context);
			placeText.Text = "Where";
			placeText.Gravity = GravityFlags.Left;
			placeText.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			placeText.SetPadding(50, 10, 50, 10);

			TextView placeDisp = new TextView(context);
			placeDisp.Text = place;
			placeDisp.Gravity = GravityFlags.Left;
			placeDisp.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 2.0f);
			placeDisp.SetPadding(50, 10, 50, 10);

			rowPlace.AddView(placeText);
			rowPlace.AddView(placeDisp);
			table.AddView(rowPlace);

			TableRow rowHost = new TableRow(context);
			rowHost.WeightSum = 3f;

			TextView hostText = new TextView(context);
			hostText.Text = "Host";
			hostText.Gravity = GravityFlags.Left;
			hostText.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			hostText.SetPadding(50, 10, 50, 10);

			TextView hostDisp = new TextView(context);
			hostDisp.Text = host;
			hostDisp.Gravity = GravityFlags.Left;
			hostDisp.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 2.0f);
			hostDisp.SetPadding(50, 10, 50, 10);

			rowHost.AddView(hostText);
			rowHost.AddView(hostDisp);
			table.AddView(rowHost);

			List<TableRow> guestRow = GuestRowGenerator(context, guests);

			if (guestRow != null)
			{
				foreach (var item in guestRow)
				{
					table.AddView(item);
				}
			}

			TableRow buttonRow = new TableRow(context);
			buttonRow.WeightSum = 3f;

			LinearLayout buttonKeeper = new LinearLayout(context);
			buttonKeeper.WeightSum = 3;
			buttonKeeper.Orientation = Orientation.Horizontal;

			Button buttonDecline = new Button(context);
			buttonDecline.Text = "Decline";
			buttonDecline.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			buttonDecline.Gravity = GravityFlags.Center;


			Button buttonClose = new Button(context);
			buttonClose.Text = "Close";
			buttonClose.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			buttonClose.Gravity = GravityFlags.Center;
			buttonClose.Click += (object sender, EventArgs e) => Dismiss();

			Button buttonAccept = new Button(context);
			buttonAccept.Text = "Accept";
			buttonAccept.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			buttonAccept.Gravity = GravityFlags.Center;


			buttonKeeper.AddView(buttonDecline);
			buttonKeeper.AddView(buttonClose);
			buttonKeeper.AddView(buttonAccept);

			table.AddView(buttonKeeper);

			SetView(table);
		}

		private List<TableRow> GuestRowGenerator(Activity context, List<string> guests)
		{
			if (guests.Count == 0)
			{
				return null;
			}

			List<TableRow> rows = new List<TableRow>();

			TableRow head = new TableRow(context);
			head.WeightSum = 3f;

			TextView guestText = new TextView(context);
			guestText.Text = String.Format("{0}", guests.Count == 1 ? "Guest:" : "Guests:");
			guestText.Gravity = GravityFlags.Left;
			guestText.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);
			guestText.SetPadding(50, 10, 50, 10);

			TextView guest = new TextView(context);
			guest.Text = String.Format("{0}", guests[0]);
			guest.Gravity = GravityFlags.Left;
			guest.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 2.0f);
			guest.SetPadding(50, 10, 50, 10);

			head.AddView(guestText);
			head.AddView(guest);

			rows.Add(head);

			for (int guestNumber = 1; guestNumber < guests.Count; guestNumber++)
			{
				TableRow body = new TableRow(context);
				body.WeightSum = 3f;

				TextView empty = new TextView(context);
				empty.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 1.0f);

				TextView additionalGuest = new TextView(context);
				additionalGuest.Text = String.Format("{0}", guests[guestNumber]);
				additionalGuest.Gravity = GravityFlags.Left;
				additionalGuest.LayoutParameters = new TableRow.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 2.0f);
				additionalGuest.SetPadding(50, 10, 50, 10);

				body.AddView(empty);
				body.AddView(additionalGuest);
				rows.Add(body);
			}

			return rows;
		}
	}
}

