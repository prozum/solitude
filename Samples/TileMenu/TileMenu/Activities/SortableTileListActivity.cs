
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

namespace TileMenu
{
	public abstract class SortableTileListActivity<T, U> : TileListActivity<T, U> where T : BaseTileListAdapter<U>
	{
		protected Spinner SortSpinner
		{
			get;
			set;
		}

		int spinnerID;

		public SortableTileListActivity(int contentViewID, int listViewID, int spinnerID) : base(contentViewID, listViewID)
		{
			this.spinnerID = spinnerID;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SortSpinner = FindViewById<Spinner>(spinnerID);

			#region Spinner Setup
			// sort when new item is selected in spinner
			SortSpinner.ItemSelected += (sender, e) => 
				{
					ListView.CollapseGroup(Focus);
					(ListView.ExpandableListAdapter as T).Sort(((e as AdapterView.ItemSelectedEventArgs).View as TextView).Text);

				};
			#endregion
		}
	}
}

