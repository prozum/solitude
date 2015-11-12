
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
	public class InfoList : TileList<List<int>>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.OfferList"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="adapter">Adapter.</param>
		public InfoList(Context context, InfoAdapter adapter)
			: base(context, adapter)
		{
			ExpListView.GroupClick += (sender, e) => 
				{
					if (Adapter.Items[e.GroupPosition].Count != 0)
						if (ExpListView.IsGroupExpanded(e.GroupPosition)) 
							ExpListView.CollapseGroup(e.GroupPosition);
						else
							ExpListView.ExpandGroup(e.GroupPosition);
				};

			Initialize();
		}

		protected override void SetGroupIndicator()
		{
			ExpListView.SetGroupIndicator(null);
		}
	}
}

