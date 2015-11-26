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
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public int MaxSlots { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.editEventInfoLayout, null);
			layout.FindViewById<EditText>(Resource.Id.edit_name).Text = Activity.Intent.GetStringExtra("title");
			layout.FindViewById<EditText>(Resource.Id.edit_description).Text = Activity.Intent.GetStringExtra("description");
			layout.FindViewById<EditText>(Resource.Id.edit_location).Text = Activity.Intent.GetStringExtra("place");
			layout.FindViewById<EditText>(Resource.Id.edit_guests).Text = Activity.Intent.GetIntExtra("maxslots", 0).ToString();

			return layout;
		}

		public void SaveInfo()
		{
		}

		public bool IsValidData()
		{
			return true;
		}

		public void LoadInfo()
		{
			Activity.Intent.GetStringExtra("type");
			Activity.Intent.GetIntExtra("leftslots", 0);
			Activity.Intent.GetIntExtra("id", 0);
		}
	}
}