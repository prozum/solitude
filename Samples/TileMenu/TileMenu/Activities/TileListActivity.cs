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
			
		protected int Focus
		{
			get;
			set;
		}

		int contentViewID;
		int listViewID;

		public TileListActivity(int contentViewID, int listViewID)
		{
			this.contentViewID = contentViewID;
			this.listViewID = listViewID;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (contentViewID);

			ListView = FindViewById<ExpandableListView>(listViewID);

			//test
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


