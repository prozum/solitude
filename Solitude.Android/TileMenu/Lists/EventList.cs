﻿using System;
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
	public class EventList : SortableTileList<EventTest>
	{
		#region Constructor
		public EventList(Context context, EventListAdapter adapter)
			: base(context, adapter, new string[] 
				{
					"Title (A-Z)",
					"Title (Z-A)",
					"Date (Soonest)",
					"Date (Lastest)",
					"Distance (Closest)",
					"Distance (Farthest)"
				})
		{
			adapter.OnCancel = (s, e) =>
				{
					ExpListView.CollapseGroup(Focus);
					RemoveFocus();
				};

			Initialize();
		}
		#endregion

	}
}
