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
	/// <summary>
	/// The fragment that contains the name, location, description and maxslot editing textfields.
	/// </summary>
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

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			var layout = inflater.Inflate(Resource.Layout.editEventInfoLayout, null);
			NameLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_title_layout);
			DescriptionLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_description_layout);
			LocationLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_location_layout);
			MaxSlotsLayout = layout.FindViewById<TextInputLayout>(Resource.Id.edit_guests_layout);
			Name = layout.FindViewById<EditText>(Resource.Id.edit_title);
			Description = layout.FindViewById<EditText>(Resource.Id.edit_description);
			Location = layout.FindViewById<EditText>(Resource.Id.edit_location);
			MaxSlots = layout.FindViewById<EditText>(Resource.Id.edit_guests);

			// Sets all the textfields information to be the infomation stored in the Activities Intent.
			Name.Text = Activity.Intent.GetStringExtra("title");
			Description.Text = Activity.Intent.GetStringExtra("description");
			Location.Text = Activity.Intent.GetStringExtra("place");
			MaxSlots.Text = Activity.Intent.GetIntExtra("maxslots", 0).ToString();

			return layout;
		}

		/// <summary>
		/// Saves the data contained in the textfields.
		/// </summary>
		public override void SaveInfo()
		{
			int res;
			int.TryParse(MaxSlots.Text, out res);

			// Put title, description, place and maxslots in the Intent of the activity 
			// which contains this fragment.
			Activity.Intent.PutExtra("title", Name.Text);
			Activity.Intent.PutExtra("description", Description.Text);
			Activity.Intent.PutExtra("place", Location.Text);
			Activity.Intent.PutExtra("maxslots", res);
		}

		/// <summary>
		/// Checks whether the textfields contains valid data.
		/// MaxSlots must be a number over 0, and the textfields just have to
		/// contains some information.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			int res;
			// Validdating info.
			var nameisvalid = !string.IsNullOrEmpty(Name.Text);
            var descisvalid = !string.IsNullOrEmpty(Description.Text);
			var localisvalid = !string.IsNullOrEmpty(Location.Text);
			var maxisvalid = int.TryParse(MaxSlots.Text, out res) && res > 0;

			// Updating all the TextInputLayouts with all the nonevalid infomation errors.
			UpdateLayout(NameLayout, Name, Resource.String.event_error_no_title, nameisvalid);
            UpdateLayout(DescriptionLayout, Description, Resource.String.event_error_no_description, descisvalid);
			UpdateLayout(LocationLayout, Location, Resource.String.event_error_no_place, localisvalid);
			UpdateLayout(MaxSlotsLayout, MaxSlots, Resource.String.event_error_no_guest, maxisvalid);

			return nameisvalid && descisvalid && localisvalid && maxisvalid;
        }

		/// <summary>
		/// A helpermethod for updating the TextInputLayouts error messages and color.
		/// </summary>
		/// <param name="layout">The layout to update.</param>
		/// <param name="text">The textfield the layout contains.</param>
		/// <param name="message">The error message.</param>
		/// <param name="noerror">The validation bool. true means, the information is valid, false means not.</param>
		private void UpdateLayout(TextInputLayout layout, EditText text, int message, bool noerror)
		{
            layout.ErrorEnabled = !noerror;

			if (noerror)
			{
				// There should be no message, when no error occured, and the color should be green.
                layout.Error = string.Empty;
                text.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
			}
			else
			{
				// There should be a message, when an error occured, and the color should be red.
				layout.Error = Resources.GetString(message);
                text.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);
			}
		}
	}
}