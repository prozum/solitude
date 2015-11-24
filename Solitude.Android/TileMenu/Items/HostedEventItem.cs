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
	public class HostedEventItem : TileListItem
	{
		#region Fields
		protected Button LeaveButton { get; set; }
		protected Button EditButton { get; set; }
		#endregion

		#region Contructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.EventItem"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="onCancel">On cancel.</param>
		public HostedEventItem(Context context, EventHandler onCancel, EventHandler onEdit)
			: base(context)
		{
			LeaveButton = new Button(context);
			EditButton = new Button(context);

			LeaveButton.Text = "Cancel";
			LeaveButton.Click += onCancel;

			EditButton.Text = "Edit";
			EditButton.Click += onEdit;

			Initialize();
		}
		#endregion

		#region Private Methods
		protected override void Initialize()
		{
			var buttonLayout = new LinearLayout(Context);
			buttonLayout.Orientation = Orientation.Horizontal;

			AddView(DescritionView);
			AddView(buttonLayout);
			buttonLayout.AddView(LeaveButton);
			buttonLayout.AddView(EditButton);

			LeaveButton.LayoutParameters.Width = -2;
			EditButton.LayoutParameters.Width = -2;

			base.Initialize();
		}
		#endregion
	}
}