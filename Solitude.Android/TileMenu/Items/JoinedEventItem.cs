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
	public class JoinedEventItem : TileListItem
	{
		#region Fields
		protected Button LeaveButton { get; set; }
		#endregion

		#region Contructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.EventItem"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="onCancel">On cancel.</param>
		public JoinedEventItem(Context context, EventHandler onLeave)
			: base(context)
		{
			LeaveButton = new Button(context);
			LeaveButton = new Button(context);

			LeaveButton.Text = "Leave";
			LeaveButton.Click += onLeave;

			Initialize();
		}
		#endregion

		#region Private Methods
		protected override void Initialize()
		{
			AddView(DescritionView);
			AddView(LeaveButton);

			LeaveButton.LayoutParameters.Width = -2;

			base.Initialize();
		}
		#endregion
	}
}