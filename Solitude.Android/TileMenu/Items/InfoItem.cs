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
	public class InfoItem : TileListItem
	{
		#region Fields
		public override string Descrition { set { DescritionView.Text = value; } }
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.OfferItem"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="onAccept">On accept.</param>
		/// <param name="onDecline">On decline.</param>
		public InfoItem(Context context)
			: base(context)
		{
			Initialize();
		}
		#endregion

		#region Public Methods
		public void SetSeperatorVisibility(ViewStates state)
		{
			SeperatorView.Visibility = state;
		}
		#endregion

		#region Private Methods
		protected override void Initialize()
		{
			AddView(DescritionView);
			DescritionView.TextSize = 18;

			base.Initialize();
		}
		#endregion
	}
}
