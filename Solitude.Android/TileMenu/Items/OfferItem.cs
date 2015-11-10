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
	public class OfferItem : TileListItem
	{
		#region Fields
		protected Button AcceptButton {	get; set; }
		protected Button DeclineButton { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.OfferItem"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="onAccept">On accept.</param>
		/// <param name="onDecline">On decline.</param>
		public OfferItem(Context context, EventHandler onAccept, EventHandler onDecline)
			: base(context)
		{
			AcceptButton = new Button(context);
			DeclineButton = new Button(context);

			AcceptButton.Text = "Accept";
			DeclineButton.Text = "Decline";
			AcceptButton.Click += onAccept;
			DeclineButton.Click += onDecline;

			Initialize();
		}
		#endregion

		#region Private Methods
		protected override void Initialize()
		{
			var buttonlayout = new LinearLayout(Context);
			buttonlayout.Orientation = Orientation.Horizontal;

			AddView(DescritionView);
			AddView(buttonlayout);
			buttonlayout.AddView(AcceptButton);
			buttonlayout.AddView(DeclineButton);

			buttonlayout.LayoutParameters.Width = -1;
			AcceptButton.LayoutParameters.Width = -2;
			DeclineButton.LayoutParameters.Width = -2;

			base.Initialize();
		}
		#endregion
	}
}