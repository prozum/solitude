
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

namespace DineWithaDane.Droid
{
	public class InfoList : TileList<List<int>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.OfferList"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adapter">Adapter.</param>
		public InfoList(Context context, InfoAdapter adapter)
			: base(context, adapter)
		{
			Initialize();
		}
	}
}

