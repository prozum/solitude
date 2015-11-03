﻿
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
	public class EventList : SortableTileList<Event>
	{
		public EventList(Context context, EventListAdapter adapter)
			: base(context, adapter)
		{
			adapter.OnCancel = (s, e) =>
				{
					ExpListView.CollapseGroup(Focus);
					RemoveFocus();
				};

			Initialize();
		}

		protected override void Initialize()
		{
			base.Initialize();
		}
	}
}

