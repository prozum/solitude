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
	public class JoinedEventList : SortableTileList<Event>
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventList"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adapter">Adapter.</param>
		public JoinedEventList(Context context, JoinedEventListAdapter adapter, EventHandler onLeave)
			: base(context, adapter, SortItems)
		{
			adapter.OnLeave = onLeave;;

			Initialize();
		}
		#endregion

	}
}

