using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Droid
{
	public class DrawerAdapter : BaseAdapter
	{
		#region Fields
		protected Activity Context { get; private set; }

		/// <summary>
		/// The values the adapter will populate views with.
		/// </summary>
		public Tuple<int, Type>[] Activities { get; private set; }
		public string[] Names { get; private set; }

		/// <summary>
		/// How many items are in the data set represented by this Adapter.
		/// </summary>
		/// <value>To be added.</value>
		public override int Count { get { return Activities.Length; } }
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.BaseTileListAdapter`1"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="items">Items.</param>
		public DrawerAdapter(Activity context, Tuple<int, Type>[] activities, string[] names) 
			: base()
		{
			Context = context;
			Activities = activities;
			Names = names;

			if (Activities.Length != Names.Length)
				throw new ArgumentException("Names and Activites must be of sames length");
		}
		#endregion


		#region Public Methods
		/// <param name="position">The position of the item within the adapter's data set of the item whose view
		///  we want.</param>
		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView; // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = Context.LayoutInflater.Inflate(Resource.Layout.DrawerItem, null);

			view.FindViewById<TextView>(Resource.Id.Text).Text = Names[position];
			view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Activities[position].Item1);

			return view;
		}

		/// <summary>
		/// Don't call this method.
		/// </summary>
		public override Java.Lang.Object GetItem(int position)
		{
			throw new NotImplementedException();
		}

		public override long GetItemId(int position)
		{
			return position;
		}
		#endregion
	}
}

