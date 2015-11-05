
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
		public TileList<InfoTest> InfoTileList { get; set; }
		public Button EditProfileButton { get; set; }
		#endregion


		#region Constructors
		public ProfileView(Context context, string name, int age, string address, TileList<InfoTest> infolist)
			: base(context)
		{
			var detailLayout = new RelativeLayout(context);
			var seperator = new LinearLayout(context);
			var septext = new TextView(context);
			var sepline = new TextView(context);
			var nameparams = new RelativeLayout.LayoutParams(-1, -2);
			var ageparams = new RelativeLayout.LayoutParams(-1, -2);
			var addressparams = new RelativeLayout.LayoutParams(-1, -2);
			var seperatorparams = new RelativeLayout.LayoutParams(-1, -2);
			var infolistparams = new RelativeLayout.LayoutParams(-1, -2);
			var editbuttonparams = new RelativeLayout.LayoutParams(-1, -2);

			seperator.Orientation = Orientation.Horizontal;
			seperator.AddView(septext);
			seperator.AddView(sepline);

			septext.Text = "Info:";
			septext.LayoutParameters.Width = -1;
			septext.Gravity = GravityFlags.Center;

			sepline.SetBackgroundColor(new Color(255,255,255));
			sepline.LayoutParameters.Width = -1;
			sepline.SetMinHeight(1);
			sepline.SetMaxHeight(1);
			sepline.Gravity = GravityFlags.Center;

			PictureView = new ImageView(context);
			NameView = new TextView(context);
			AgeView = new TextView(context);
			AddressView = new TextView(context);
			InfoTileList = infolist;
			EditProfileButton = new Button(context);

			NameView.Text = "Name: " + name;
			AgeView.Text = "Age: " + age;
			AddressView.Text = "Address: " + address;

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
			AddView(EditProfileButton);

			detailLayout.Id = 4;
			PictureView.Id = 5;
			NameView.Id = 6;
			AgeView.Id = 7;
			AddressView.Id = 8;
			seperator.Id = 9;
			InfoTileList.Id = 10;
			EditProfileButton.Id = 11;

			nameparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			ageparams.AddRule(LayoutRules.Below, NameView.Id);
			ageparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.Below, AgeView.Id);
			addressparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			seperatorparams.AddRule(LayoutRules.Below, PictureView.Id);
			infolistparams.AddRule(LayoutRules.Above, EditProfileButton.Id);
			infolistparams.AddRule(LayoutRules.Below, detailLayout.Id);
			editbuttonparams.AddRule(LayoutRules.AlignParentBottom);

			NameView.LayoutParameters = nameparams;
			AgeView.LayoutParameters = ageparams;
			AddressView.LayoutParameters = addressparams;
			seperator.LayoutParameters = seperatorparams;
			InfoTileList.LayoutParameters = infolistparams;
			EditProfileButton.LayoutParameters = editbuttonparams;
		}
		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion
	}
}

