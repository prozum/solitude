using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;

namespace Solitude.Droid
{
	public class HostedEventListAdapter : BaseCardAdapter<Event>
	{
		#region Fields
		/// <summary>
		/// Gets or sets the handler for the cancel button of the views.
		/// </summary>
		public EventHandler OnCancel { get; set; }
		public EventHandler OnEdit { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Solitude.Droid.EventListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public HostedEventListAdapter(Activity context, List<Event> items) 
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