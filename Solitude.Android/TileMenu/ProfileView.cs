/*
 * Don't mind me, im dumb.
 * 						- Jimmi
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android;
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
		protected ImageView PictureView { get; set; }
		protected TextView NameView { get; set; }
		protected TextView AddressView { get; set; }
		#endregion


		#region Constructors
		public ProfileView(Context context, User user)
			: base(context)
		{
			#region Initiating Views and Params
			var detailLayout = new RelativeLayout(context);
			var scrollview = new ScrollView(context);
			var infolayout = new LinearLayout(context);
			var languagetext = new TextView(context);
			var intereststext = new TextView(context);
			var foodhabitstext = new TextView(context);
			var editdetails = new Button(context);
			var editlanguage = new Button(context);
			var editinterests = new Button(context);
			var editfoodhabits = new Button(context);

			var nameparams = new RelativeLayout.LayoutParams(-1, -2);
			var addressparams = new RelativeLayout.LayoutParams(-1, -2);
			var editdetailsparams = new RelativeLayout.LayoutParams(-2, -2);
			#endregion

			#region Adding parameters to proporties
			User = user;
			PictureView = new ImageView(context);
			NameView = new TextView(context);
			AddressView = new TextView(context);
			#endregion

			#region Setting variables values
			Orientation = Orientation.Vertical;
			infolayout.Orientation = Orientation.Vertical;
			infolayout.SetHorizontalGravity(GravityFlags.Right);

			NameView.Text = "Name: " + User.Name;
			AddressView.Text = "Address: " + User.Address;
			languagetext.Text = "Languages:";
			intereststext.Text = "\nInterests:";
			foodhabitstext.Text = "\nFood Habits:";
			editdetails.Text = "Edit";
			editlanguage.Text = "Edit";
			editinterests.Text = "Edit";
			editfoodhabits.Text = "Edit";

			PictureView.SetImageResource(Resource.Drawable.Icon);
			PictureView.SetMinimumHeight(100);
			PictureView.SetMinimumWidth(100);

			PictureView.Id = 5;
			NameView.Id = 6;
			AddressView.Id = 7;

			editdetails.Click += EditDetails;
			editlanguage.Click += EditLanguage;
			editinterests.Click += EditInterests;
			editfoodhabits.Click += EditFoodHabits;
			#endregion

			#region Adding views to layouts
			detailLayout.AddView(PictureView);
			detailLayout.AddView(NameView);
			detailLayout.AddView(AddressView);
			detailLayout.AddView(editdetails);

			infolayout.AddView(languagetext);
			infolayout.AddView(new Seperator(context, Color.Black));
			AddTo(infolayout, Enum.GetNames(typeof(Language)), context);
			infolayout.AddView(editlanguage);

			infolayout.AddView(intereststext);
			infolayout.AddView(new Seperator(context, Color.Black));
			AddTo(infolayout, Enum.GetNames(typeof(Interest)), context);
			infolayout.AddView(editinterests);

			infolayout.AddView(foodhabitstext);
			infolayout.AddView(new Seperator(context, Color.Black));
			AddTo(infolayout, Enum.GetNames(typeof(FoodHabit)), context);
			infolayout.AddView(editfoodhabits);

			scrollview.AddView(infolayout);

			AddView(detailLayout);
			AddView(new Seperator(context, Color.Black));
			AddView(scrollview);
			#endregion

			#region Adding LayoutParams
			scrollview.LayoutParameters.Height = -1;
			infolayout.LayoutParameters.Height = -1;
			editlanguage.LayoutParameters.Width = -2;
			editinterests.LayoutParameters.Width = -2;
			editfoodhabits.LayoutParameters.Width = -2;

			nameparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.RightOf, PictureView.Id);
			addressparams.AddRule(LayoutRules.Below, NameView.Id);
			editdetailsparams.AddRule(LayoutRules.Below, PictureView.Id);
			editdetailsparams.AddRule(LayoutRules.Below, AddressView.Id);
			editdetailsparams.AddRule(LayoutRules.AlignParentRight);

			NameView.LayoutParameters = nameparams;
			AddressView.LayoutParameters = addressparams;
			editdetails.LayoutParameters = editdetailsparams;
			#endregion

		}
		#endregion


		#region Public Methods

		#endregion


		#region Private Methods
		private void AddTo(ViewGroup layout, IEnumerable<object> items, Context context)
		{
			foreach (var item in items)
			{
				var view = new TextView(context);
				view.Text = item.ToString();
				layout.AddView(view);
			}
		}

		private void EditDetails(object sender, EventArgs evntarg)
		{
			var dialog = new Dialog(Context);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditDetails);

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

		private void EditLanguage(object sender, EventArgs evntarg)
		{
			var dialog = new Dialog(Context);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditComparableInfo);

			var nameview = dialog.FindViewById<TextView>(Resource.Id.Text);
			var infolist = dialog.FindViewById<ListView>(Resource.Id.InfoList);
			var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
			var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

			nameview.Text = "Languages:";
			infolist.Adapter = new ArrayAdapter<string>(Context, Resource.Layout.CheckedListViewItem, Enum.GetNames(typeof(Language)));

			savebutton.Click += (s, e) => 
				{
					dialog.Dismiss();
				};

			cancelbutton.Click += (s, e) => 
				{
					dialog.Dismiss();
				};

			dialog.Show();
		}

		private void EditInterests(object sender, EventArgs evntarg)
		{
			var dialog = new Dialog(Context);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditComparableInfo);

			var nameview = dialog.FindViewById<TextView>(Resource.Id.Text);
			var infolist = dialog.FindViewById<ListView>(Resource.Id.InfoList);
			var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
			var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

			nameview.Text = "Interests:";
			infolist.Adapter = new ArrayAdapter<string>(Context, Resource.Layout.CheckedListViewItem, Enum.GetNames(typeof(Interest)));

			savebutton.Click += (s, e) => 
				{
					dialog.Dismiss();
				};

			cancelbutton.Click += (s, e) => 
				{
					dialog.Dismiss();
				};

			dialog.Show();
		}

		private void EditFoodHabits(object sender, EventArgs evntarg)
		{
			var dialog = new Dialog(Context);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditComparableInfo);

			var nameview = dialog.FindViewById<TextView>(Resource.Id.Text);
			var infolist = dialog.FindViewById<ListView>(Resource.Id.InfoList);
			var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
			var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

			nameview.Text = "Food Habits:";
			infolist.Adapter = new ArrayAdapter<string>(Context, Resource.Layout.CheckedListViewItem, Enum.GetNames(typeof(FoodHabit)));

			savebutton.Click += (s, e) => 
				{
					dialog.Dismiss();
				};

			cancelbutton.Click += (s, e) => 
				{
					dialog.Dismiss();
				};

			dialog.Show();
		}
		#endregion
	}
}

