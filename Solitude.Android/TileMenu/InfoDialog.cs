
using System;
using System.Collections;
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
	public class InfoDialog : Dialog
	{
		public Button SaveButton { get; set; }
		public Button CancelButton { get; set; }
		public ListView InfoList { get; set; }

		public InfoDialog(Context context, InfoType type, IList info)
			: base(context)
		{
			RequestWindowFeature((int)WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.EditComparableInfo);

			// getting relevant views
			var nameview = FindViewById<TextView>(Resource.Id.Text);
			InfoList = FindViewById<ListView>(Resource.Id.InfoList);
			SaveButton = FindViewById<Button>(Resource.Id.SaveButton);
			CancelButton = FindViewById<Button>(Resource.Id.CancelButton);

			// setting header text info
			nameview.Text = ProfileView.Titles[(int)type];

			// setting up list of info with checkboxes
			InfoList.Adapter = new ArrayAdapter<string>(context, Resource.Layout.CheckedListViewItem, ProfileView.Names[(int)type]);
			InfoList.ChoiceMode = ChoiceMode.Multiple;

			// selecting info that is already chosen by user
			foreach (var item in info)
				InfoList.SetItemChecked((int)item, true);
		}

		public List<int> ItemsChecked()
		{
			var res = new List<int>();

			for (int i = 0; i < InfoList.ChildCount; i++) 
				if ((InfoList.GetChildAt(i) as CheckBox).Checked)
					res.Add(i);

			return res;
		}

		public void ItemsChecked(IList info)
		{
			info.Clear();

			for (int i = 0; i < InfoList.ChildCount; i++) 
				if ((InfoList.GetChildAt(i) as CheckBox).Checked)
					info.Add(i);
		}
	}
}

