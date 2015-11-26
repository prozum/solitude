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

namespace Solitude.Droid
{
	public class EventInfoFragment : Android.Support.V4.App.Fragment, IEditPage
	{
		public EditText Name { get; set; }
		public EditText Description { get; set; }
		public EditText Location { get; set; }
		public EditText MaxSlots { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.editEventInfoLayout, null);
			Name = layout.FindViewById<EditText>(Resource.Id.edit_name);
			Description = layout.FindViewById<EditText>(Resource.Id.edit_description);
			Location = layout.FindViewById<EditText>(Resource.Id.edit_location);
			MaxSlots = layout.FindViewById<EditText>(Resource.Id.edit_guests);

			Name.Text = Activity.Intent.GetStringExtra("title");
			Description.Text = Activity.Intent.GetStringExtra("description");
			Location.Text = Activity.Intent.GetStringExtra("place");
			MaxSlots.Text = Activity.Intent.GetIntExtra("maxslots", 0).ToString();

			return layout;
		}

		public void SaveInfo()
		{
			int res;
			int.TryParse(MaxSlots.Text, out res);

			Activity.Intent.PutExtra("title", Name.Text);
			Activity.Intent.PutExtra("description", Description.Text);
			Activity.Intent.PutExtra("place", Location.Text);
			Activity.Intent.PutExtra("maxslots", res);
		}

		public bool IsValidData()
		{
			int res;
			var nameisvalid = string.IsNullOrEmpty(Name.Text);
            var descisvalid = string.IsNullOrEmpty(Description.Text);
			var localisvalid = string.IsNullOrEmpty(Location.Text);
			var maxisvalid = int.TryParse(MaxSlots.Text, out res);

			return nameisvalid && descisvalid && localisvalid && maxisvalid;
        }

		public void LoadInfo()
		{
			Activity.Intent.GetStringExtra("type");
			Activity.Intent.GetIntExtra("leftslots", 0);
			Activity.Intent.GetIntExtra("id", 0);
		}
	}
}