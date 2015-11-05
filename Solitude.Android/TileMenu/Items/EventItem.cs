
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
	public class EventItem : TileListItem
	{
		protected Button CancelButton
		{
			get;
			set;
		}

		public EventItem(Context context, EventHandler onCancel)
			: base(context)
		{
			CancelButton = new Button(context);

			CancelButton.Text = "Cancel";
			CancelButton.Click += onCancel;

			Initialize();
		}

		protected override void Initialize()
		{
			AddView(DescritionView);
			AddView(CancelButton);

			CancelButton.LayoutParameters.Width = -2;

			base.Initialize();
		}
	}
}

