
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
		public ImageView PictureView { get; set; }
		public TextView NameView { get; set; }
		public TextView AgeView { get; set; }
		public TextView AddressView { get; set; }
		public InfoList InfoTileList { get; set; }
		public Button EditProfileButton { get; set; }
		#endregion


		#region Constructors
		public ProfileView(Context context, string name, int age, string address, InfoList infolist)
			: base(context)
		{
			var detailLayout = new RelativeLayout(context);
			var seperator = new LinearLayout(context);
			var septext = new TextView(context);
			var sepline = new TextView(context);
			var sepline2 = new TextView(context);

			var nameparams = new RelativeLayout.LayoutParams(-1, -2);
			var ageparams = new RelativeLayout.LayoutParams(-1, -2);
			var addressparams = new RelativeLayout.LayoutParams(-1, -2);
			var seperatorparams = new RelativeLayout.LayoutParams(-1, -2);
			var infolistparams = new RelativeLayout.LayoutParams(-1, -2);
			var sepline2params = new RelativeLayout.LayoutParams(-1, 1);
			var editbuttonparams = new RelativeLayout.LayoutParams(-2, -2);

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

			PictureView = new ImageView(context);
			NameView = new TextView(context);
			AgeView = new TextView(context);
			AddressView = new TextView(context);
			InfoTileList = infolist;
			EditProfileButton = new Button(context);

			NameView.Text = "Name: " + name;
			AgeView.Text = "Age: " + age;
			AddressView.Text = "Address: " + address;
			EditProfileButton.Text = "Edit Profile";

			PictureView.SetImageResource(Resource.Drawable.Icon);
			PictureView.SetMinimumHeight(100);
			PictureView.SetMinimumWidth(100);

			detailLayout.AddView(PictureView);
			detailLayout.AddView(NameView);
			detailLayout.AddView(AgeView);
			detailLayout.AddView(AddressView);
			detailLayout.AddView(seperator);

			AddView(detailLayout);
			AddView(InfoTileList);
			AddView(sepline2);
			AddView(EditProfileButton);

			detailLayout.Id = 4;
			PictureView.Id = 5;
			NameView.Id = 6;
			AgeView.Id = 7;
			AddressView.Id = 8;
			seperator.Id = 9;
			InfoTileList.Id = 10;
			sepline2.Id = 11;
			EditProfileButton.Id = 12;

			nameparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			ageparams.AddRule(LayoutRules.Below, NameView.Id);
			ageparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.Below, AgeView.Id);
			addressparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			seperatorparams.AddRule(LayoutRules.Below, PictureView.Id);
			infolistparams.AddRule(LayoutRules.Above, sepline2.Id);
			infolistparams.AddRule(LayoutRules.Below, detailLayout.Id);
			sepline2params.AddRule(LayoutRules.Above, EditProfileButton.Id);
			editbuttonparams.AddRule(LayoutRules.AlignParentBottom);
			editbuttonparams.AddRule(LayoutRules.AlignParentRight);

			NameView.LayoutParameters = nameparams;
			AgeView.LayoutParameters = ageparams;
			AddressView.LayoutParameters = addressparams;
			seperator.LayoutParameters = seperatorparams;
			InfoTileList.LayoutParameters = infolistparams;
			sepline2.LayoutParameters = sepline2params;
			EditProfileButton.LayoutParameters = editbuttonparams;

			EditProfileButton.Click += EditClick;
		}
		#endregion


		#region Public Methods

		#endregion


		#region Private Methods
		private void EditClick(object sender, EventArgs e)
		{
			if (InfoTileList.EditMode)
			{
				InfoTileList.EditMode = false;
				EditProfileButton.Text = "Edit Profile";
			}
			else
			{
				InfoTileList.EditMode = true;
				EditProfileButton.Text = "Done";
			}
		}
		#endregion
	}
}

