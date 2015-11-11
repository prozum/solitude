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
	public class OfferList : SortableTileList<Event>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.OfferList"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adapter">Adapter.</param>
		public OfferList(Context context, OfferListAdapter adapter, EventHandler onAccept, EventHandler onDecline)
			: base(context, adapter, SortItems)
		{
			adapter.OnAccept = onAccept;
			adapter.OnDecline = onDecline;

			Initialize();
		}
		#endregion
	}
}

