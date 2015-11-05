
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
	public class InfoList : TileList<InfoTest>
	{
		public bool EditMode
		{
			get;
			set;
		}

		public InfoList(Context context, ProfileInfoListAdapter adapter)
			: base(context, adapter)
		{
			Initialize();
		}
	}
}

