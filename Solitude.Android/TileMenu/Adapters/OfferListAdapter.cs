using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Solitude.Droid
{
	public class OfferListAdapter : BaseCardAdapter<Offer>
	{
		#region Fields
		/// <summary>
		/// Gets or sets the handler for the accept button of the views.
		/// </summary>
		public EventHandler OnAccept { get; set; }

		/// <summary>
		/// Gets or sets the handler for the decline button of the views.
		/// </summary>
		public EventHandler OnDecline { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Solitude.Droid.OfferListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public OfferListAdapter(Activity context, List<Offer> items) 
			: base(context, items) { }
		#endregion


		#region Public Methods
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}