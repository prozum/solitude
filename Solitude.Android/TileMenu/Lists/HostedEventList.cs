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
	public class HostedEventList : SortableTileList<Event>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventList"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adapter">Adapter.</param>
		public HostedEventList(Context context, HostedEventListAdapter adapter, EventHandler onCancel, EventHandler onEdit)
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
			adapter.OnCancel = onCancel;
			adapter.OnEdit = onEdit;

			Initialize();
		}
		#endregion

	}
}

