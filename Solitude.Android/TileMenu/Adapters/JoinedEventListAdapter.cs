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
	public class JoinedEventListAdapter : BaseCardAdapter<Event>
	{
		#region Fields
		/// <summary>
		/// Gets or sets the handler for the cancel button of the views.
		/// </summary>
		protected EventHandler OnLeave { get; set; }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Solitude.Droid.EventListAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public JoinedEventListAdapter(Activity context, List<Event> items, EventHandler onLeave) 
			: base(context, items)
		{
			OnLeave = onLeave;
        }
		#endregion


		#region Public Methods
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView; // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = Context.LayoutInflater.Inflate(Resource.Layout.JoinedCard, null);

			view.FindViewById<TextView>(Resource.Id.title).Text = Items[position].Title;

			/*
			// set view information
			view.Title = Items[groupPosition].Title;
			view.Place = Items[groupPosition].Address;
			view.Date = Items[groupPosition].Date;
			view.Slots = new Tuple<int, int>(Items[groupPosition].SlotsTaken, Items[groupPosition].SlotsTotal);
			*/

			var expandtext = view.FindViewById<TextView>(Resource.Id.expanded_content);
			var expander = view.FindViewById<ImageView>(Resource.Id.expander);

			return view;
		}
		#endregion
	}
}