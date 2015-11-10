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
	public class InfoList : TileList<InfoTest>
	{
		#region Fields
		public bool EditMode { get;	set; }
		#endregion

		#region Constructors
		public InfoList(Context context, ProfileInfoListAdapter adapter)
			: base(context, adapter)
		{
			Initialize();

			ExpListView.GroupClick += InfoClick;
		}
		#endregion

		#region Private Methods
		private void InfoClick(object sender, ExpandableListView.GroupClickEventArgs evntarg)
		{
			if (EditMode) 
			{
				var dialog = new Dialog(Context);
				dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
				dialog.SetContentView(Resource.Layout.EditDetails);

				var nameview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.NameEditor);
				var descview = dialog.FindViewById<MultiAutoCompleteTextView>(Resource.Id.AddressEditor);
				var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
				var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

				nameview.Text = Adapter.Items[evntarg.GroupPosition].Name;
				descview.Text = Adapter.Items[evntarg.GroupPosition].Description;

				savebutton.Click += (s, e) => 
					{
						Adapter.Items[evntarg.GroupPosition].Name = nameview.Text;
						Adapter.Items[evntarg.GroupPosition].Description = descview.Text;

						Adapter.NotifyDataSetChanged();
						dialog.Dismiss();
					};

				cancelbutton.Click += (s, e) => 
					{
						dialog.Dismiss();
					};

				dialog.Show();
			}
			else
			{
				if (ExpListView.IsGroupExpanded(evntarg.GroupPosition))
					ExpListView.CollapseGroup(evntarg.GroupPosition);
				else
					ExpListView.ExpandGroup(evntarg.GroupPosition);
			}
		}
		#endregion
	}
}