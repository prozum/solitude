using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Android
{
	public class ProfileInfoListAdapter : BaseTileListAdapter<InfoTest>
	{
		#region Constructors
		public ProfileInfoListAdapter(Activity context, List<InfoTest> items) 
			: base(context, items) { }
		#endregion

		#region Public Methods
		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			var view = (convertView as InfoGroup); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new InfoGroup(Context);

			// set view information
			view.InfoName = Items[groupPosition].Name;

			// set seperator visibility
			if (isExpanded)
				view.SeperatorVisibility(ViewStates.Gone);
			else
				view.SeperatorVisibility(ViewStates.Visible);

			return view;
		}

		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			var view = (convertView as InfoItem); // re-use an existing view, if one is available

			if (view == null)// otherwise create a new one
				view = new InfoItem(Context);

			// set view information
			view.Descrition = Items[groupPosition].Description;

			return view;
		}

		public override void Sort(string context)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}