using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Solitude.Droid.TileMenu.Adapters
{
	/*
	public class InfoAdapter : BaseAdapter
	{
		#region Fields
		protected Activity Context { get; private set; }

		public string[] Names { get; private set; }
		public override int Count { get { return Activities.Length; } }
		#endregion


		#region Constructors
		public InfoAdapter(Activity context, string[] names) 
			: base()
		{
			Context = context;
			Names = names;
		}
		#endregion


		#region Public Methods
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView; // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = Context.LayoutInflater.Inflate(Resource.Layout, null);

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
	*/
}