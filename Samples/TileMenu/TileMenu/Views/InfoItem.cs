
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
	public class InfoItem : TileListItem
	{
		public InfoItem(Context context)
			: base(context)
		{
			Initialize();
		}

		protected override void Initialize()
		{
			AddView(DescritionView);

			base.Initialize();
		}
	}
}

