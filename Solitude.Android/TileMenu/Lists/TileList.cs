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
	public abstract class TileList<T> : RelativeLayout
	{
		#region Fields
		protected ExpandableListView ExpListView { get; set; }
		protected BaseTileListAdapter<T> Adapter { get;	set; }
		protected int Focus	{ get; set;	}
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.TileList`1"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adabter">Adabter.</param>
		public TileList(Context context, BaseTileListAdapter<T> adabter)
			: base(context)
		{
			Adapter = adabter;

			ExpListView = new ExpandableListView(Context);
			ExpListView.SetAdapter(Adapter);
			ExpListView.SetGroupIndicator(null);

			// focus selfcollapse, when another item expands
			ExpListView.GroupExpand += (sender, e) => 
				{
					if (Focus != e.GroupPosition) 
					{
						ExpListView.CollapseGroup(Focus);
						Focus = e.GroupPosition;
					}
				};
		}
		#endregion

		#region Public Methods
		public T PopFocus()
		{
			var res = GetFocus();
			RemoveFocus();

			return res;
		}

		public void RemoveFocus()
		{
			Adapter.RemoveAt(Focus);
		}

		public T GetFocus()
		{
			return Adapter.Items[Focus];
		}
		#endregion

		#region Private Methods

		protected virtual void Initialize()
		{
			AddView(ExpListView);

			ExpListView.LayoutParameters.Width = -1;
		}
		#endregion

	}
}