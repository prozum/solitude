using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TileMenu
{
	public abstract class TileListActivity<T, U> : Activity where T : BaseTileListAdapter<U>
	{
		protected ExpandableListView ListView
		{
			get;
			set;
		}
		protected Spinner Spinner
		{
			get;
			set;
		}
			
		protected int Focus
		{
			get;
			set;
		}

		int contentViewID;
		int spinnerID;
		int listViewID;

		public TileListActivity(int contentViewID, int spinnerID, int listViewID)
		{
			this.contentViewID = contentViewID;
			this.spinnerID = spinnerID;
			this.listViewID = listViewID;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (contentViewID);

			Spinner = FindViewById<Spinner>(spinnerID);
			ListView = FindViewById<ExpandableListView>(listViewID);

			#region Spinner Setup
			// sort when new item is selected in spinner
			Spinner.ItemSelected += (sender, e) => 
				{
					ListView.CollapseGroup(Focus);
					(ListView.ExpandableListAdapter as T).Sort(((e as AdapterView.ItemSelectedEventArgs).View as TextView).Text);

				};
			#endregion

			#region ListView Setup
			// focus selfcollapse, when another item expands
			ListView.GroupExpand += (sender, e) => 
				{
					if (Focus != e.GroupPosition) {
						ListView.CollapseGroup(Focus);
						Focus = e.GroupPosition;
					}
				};
			#endregion
		}

		protected virtual void OnButton1(object sender, EventArgs e)
		{
			(ListView.ExpandableListAdapter as T).RemoveAt(Focus);
			ListView.CollapseGroup(Focus);
		}

		protected virtual void OnButton2(object sender, EventArgs e)
		{
			(ListView.ExpandableListAdapter as T).RemoveAt(Focus);
			ListView.CollapseGroup(Focus);
		}
	}
}


