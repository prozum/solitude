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
using Android.Support.Design.Widget;
using Android.Graphics;
using Android.Views.InputMethods;

namespace Solitude.Droid
{
	public class EventInfoFragment : EditFragment
	{
		public TextInputLayout NameLayout { get; set; }
		public TextInputLayout DescriptionLayout { get; set; }
		public TextInputLayout LocationLayout { get; set; }
		public TextInputLayout MaxSlotsLayout { get; set; }
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
			NameLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_title_layout);
			DescriptionLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_description_layout);
			LocationLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_location_layout);
			MaxSlotsLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_guests_layout);
			Name = layout.FindViewById<EditText>(Resource.Id.edit_title);
			Description = layout.FindViewById<EditText>(Resource.Id.edit_description);
			Location = layout.FindViewById<EditText>(Resource.Id.edit_location);
			MaxSlots = layout.FindViewById<EditText>(Resource.Id.edit_guests);

			Name.Text = Activity.Intent.GetStringExtra("title");
			Description.Text = Activity.Intent.GetStringExtra("description");
			Location.Text = Activity.Intent.GetStringExtra("place");
			MaxSlots.Text = Activity.Intent.GetIntExtra("maxslots", 0).ToString();

			return layout;
		}

		public override void SaveInfo()
		{
			int res;
			int.TryParse(MaxSlots.Text, out res);

			Activity.Intent.PutExtra("title", Name.Text);
			Activity.Intent.PutExtra("description", Description.Text);
			Activity.Intent.PutExtra("place", Location.Text);
			Activity.Intent.PutExtra("maxslots", res);
		}

		public override bool IsValidData()
		{
			int res;
			var nameisvalid = !string.IsNullOrEmpty(Name.Text);
            var descisvalid = !string.IsNullOrEmpty(Description.Text);
			var localisvalid = !string.IsNullOrEmpty(Location.Text);
			var maxisvalid = int.TryParse(MaxSlots.Text, out res) && res > 0;

            UpdateLayout(NameLayout, Name, Resource.String.event_error_no_title, nameisvalid);
            UpdateLayout(DescriptionLayout, Description, Resource.String.event_error_no_description, descisvalid);
			UpdateLayout(LocationLayout, Location, Resource.String.event_error_no_place, localisvalid);
			UpdateLayout(MaxSlotsLayout, MaxSlots, Resource.String.event_error_no_guest, maxisvalid);

			return nameisvalid && descisvalid && localisvalid && maxisvalid;
        }

		private void UpdateLayout(TextInputLayout layout, EditText text, int message, bool noerror)
		{
            layout.ErrorEnabled = !noerror;

			if (noerror)
			{
                layout.Error = string.Empty;
                text.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
			}
			else
			{
                layout.Error = Resources.GetString(message);
                text.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);
			}
		}
	}
}