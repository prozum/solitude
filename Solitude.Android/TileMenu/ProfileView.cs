
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
	public class ProfileView : RelativeLayout
	{
		#region Fields
		protected User User { get; set; }
		protected bool EditMode { get; set; }
		protected ImageView PictureView { get; set; }
		protected TextView NameView { get; set; }
		protected TextView AddressView { get; set; }
		//protected InfoList InfoTileList { get; set; }
		protected Button EditProfileButton { get; set; }
		#endregion


		#region Constructors
		public ProfileView(Context context, User user, InfoList infolist)
			: base(context)
		{
			var detailLayout = new RelativeLayout(context);
			/*
			var seperator = new LinearLayout(context);
			var septext = new TextView(context);
			var sepline = new TextView(context);
			var sepline2 = new TextView(context);
			*/

			var nameparams = new RelativeLayout.LayoutParams(-1, -2);
			var addressparams = new RelativeLayout.LayoutParams(-1, -2);
			//var seperatorparams = new RelativeLayout.LayoutParams(-1, -2);
			//var infolistparams = new RelativeLayout.LayoutParams(-1, -2);
			//var sepline2params = new RelativeLayout.LayoutParams(-1, 1);
			var editbuttonparams = new RelativeLayout.LayoutParams(-2, -2);

			PictureView = new ImageView(context);
			NameView = new TextView(context);
			AddressView = new TextView(context);
			//InfoTileList = infolist;
			EditProfileButton = new Button(context);

			User = user;

			/*
			seperator.Orientation = Orientation.Vertical;
			seperator.AddView(septext);
			seperator.AddView(sepline);

			septext.Text = "Info:";
			septext.LayoutParameters.Width = -1;
			septext.Gravity = GravityFlags.Center;

			sepline.SetBackgroundColor(new Color(255,255,255));
			sepline.LayoutParameters.Width = -1;
			sepline.LayoutParameters.Height = 1;

			sepline2.SetBackgroundColor(new Color(255,255,255));
			*/

			NameView.Text = "Name: " + User.Name;
			AddressView.Text = "Address: " + User.Address;
			EditProfileButton.Text = "Edit Profile";

			PictureView.SetImageResource(Resource.Drawable.Icon);
			PictureView.SetMinimumHeight(100);
			PictureView.SetMinimumWidth(100);

			detailLayout.AddView(PictureView);
			detailLayout.AddView(NameView);
			detailLayout.AddView(AddressView);
			//detailLayout.AddView(seperator);

			AddView(detailLayout);
			//AddView(InfoTileList);
			//AddView(sepline2);
			AddView(EditProfileButton);

			detailLayout.Id = 4;
			PictureView.Id = 5;
			NameView.Id = 6;
			AddressView.Id = 7;
			//seperator.Id = 8;
			//InfoTileList.Id = 9;
			//sepline2.Id = 10;
			EditProfileButton.Id = 11;

			nameparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.Below, NameView.Id);
			//seperatorparams.AddRule(LayoutRules.Below, PictureView.Id);
			//infolistparams.AddRule(LayoutRules.Above, sepline2.Id);
			//infolistparams.AddRule(LayoutRules.Below, detailLayout.Id);
			//sepline2params.AddRule(LayoutRules.Above, EditProfileButton.Id);
			editbuttonparams.AddRule(LayoutRules.AlignParentBottom);
			editbuttonparams.AddRule(LayoutRules.AlignParentRight);

			NameView.LayoutParameters = nameparams;
			AddressView.LayoutParameters = addressparams;
			//seperator.LayoutParameters = seperatorparams;
			//InfoTileList.LayoutParameters = infolistparams;
			//sepline2.LayoutParameters = sepline2params;
			EditProfileButton.LayoutParameters = editbuttonparams;

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

