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
		#region Fields
		protected Button CancelButton { get; set; }
		#endregion

		#region Contructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.EventItem"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="onCancel">On cancel.</param>
		public EventItem(Context context, EventHandler onCancel)
			: base(context)
		{
			CancelButton = new Button(context);

			CancelButton.Text = "Cancel";
			CancelButton.Click += onCancel;

			Initialize();
		}
		#endregion

		#region Private Methods
		protected override void Initialize()
		{
			AddView(DescritionView);
			AddView(CancelButton);

			CancelButton.LayoutParameters.Width = -2;

			base.Initialize();
		}
		#endregion
	}
}