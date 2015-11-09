/*
 * Don't mind me, im dumb.
 * 						- Jimmi
 */

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
using Android.Graphics;

namespace DineWithaDane.Android
{
	public class ProfileView : LinearLayout
	{
		#region Fields
		protected User User { get; set; }
		protected bool EditMode { get; set; }
		protected ImageView PictureView { get; set; }
		protected TextView NameView { get; set; }
		protected TextView AddressView { get; set; }
		protected Button EditProfileButton { get; set; }
		#endregion


		#region Constructors
		public ProfileView(Context context, User user)
			: base(context)
		{
			var detailLayout = new RelativeLayout(context);
			var languagetext = new TextView(context);
			var intereststext = new TextView(context);
			var foodhabitstext = new TextView(context);

			var nameparams = new RelativeLayout.LayoutParams(-1, -2);
			var addressparams = new RelativeLayout.LayoutParams(-1, -2);

			PictureView = new ImageView(context);
			NameView = new TextView(context);
			AddressView = new TextView(context);
			EditProfileButton = new Button(context);

			Orientation = Orientation.Vertical;

			User = user;

			NameView.Text = "Name: " + User.Name;
			AddressView.Text = "Address: " + User.Address;
			EditProfileButton.Text = "Edit Profile";
			languagetext.Text = "Languages:";
			intereststext.Text = "Interests:";
			foodhabitstext.Text = "Food Habits:";

			PictureView.SetImageResource(Resource.Drawable.Icon);
			PictureView.SetMinimumHeight(100);
			PictureView.SetMinimumWidth(100);

			detailLayout.AddView(PictureView);
			detailLayout.AddView(NameView);
			detailLayout.AddView(AddressView);

			var test = new ListView(context);
			test.Adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SpinnerItem, Enum.GetNames(typeof(Language)));
			test.Enabled = false;

			AddView(detailLayout);
			AddView(languagetext);
			AddView(test);
			AddView(intereststext);
			AddView(foodhabitstext);
			AddView(EditProfileButton);

			PictureView.Id = 5;
			NameView.Id = 6;
			AddressView.Id = 7;

			nameparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.Below, NameView.Id);

			NameView.LayoutParameters = nameparams;
			AddressView.LayoutParameters = addressparams;

			EditProfileButton.Click += EditClick;
			NameView.Click += TextClick;
			AddressView.Click += TextClick;
		}
		#endregion


		#region Public Methods

		#endregion


		#region Private Methods
		private void EditClick(object sender, EventArgs e)
		{
			if (EditMode)
			{
				EditMode = false;
				//InfoTileList.EditMode = false;
				EditProfileButton.Text = "Edit Profile";
			}
			else
			{
				EditMode = true;
				//InfoTileList.EditMode = true;
				EditProfileButton.Text = "Done";
			}
		}

		private void TextClick(object sender, EventArgs evntarg)
		{
			if (EditMode)
			{
				var dialog = new Dialog(Context);
				dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
				dialog.SetContentView(Resource.Layout.EditText);

				var nameview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.NameEditor);
				var addressview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.AddressEditor);
				var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
				var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

				nameview.Text = User.Name;
				addressview.Text = User.Address;

				savebutton.Click += (s, e) => 
					{
						User.Name = nameview.Text;
						User.Address = addressview.Text;
						NameView.Text = "Name: " + User.Name;
						AddressView.Text = "Address: " + User.Address;

						dialog.Dismiss();
					};

				cancelbutton.Click += (s, e) => 
					{
						dialog.Dismiss();
					};

				dialog.Show();
			}
		}
		#endregion
	}
}

