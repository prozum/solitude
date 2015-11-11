/*
 * Don't mind me, im dumb.
 * 						- Jimmi
 */

using System;
using System.Collections;
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
		private static readonly string[] Titles = new string[]
			{
				"Languages",
				"Interests",
				"Food Habits"
			};

		private static readonly string[][] Names = new string[][]
			{
				new string[]
				{
					"Danish",
					"English",
					"German",
					"French",
					"Spanish",
					"Chinese",
					"Russian"
				},
				new string[]
				{
					"Nature",
					"Fitness",
					"Movies",
					"Gaming",
					"Electronics",
					"Food",
					"Drawing"
				},
				new string[]
				{
					"Halal",
					"Kosher",
					"Vegan",
					"NoLactose",
					"NoGluten",
					"NoNuts"
				}
			};

		#region Fields
		protected User User { get; set; }
		protected List<Language> Languages { get; set; }
		protected List<Interest> Interests { get; set; }
		protected List<FoodHabit> FoodHabits { get; set; }

		protected ImageView PictureView { get; set; }
		protected TextView NameView { get; set; }
		protected TextView AddressView { get; set; }

		protected LinearLayout LanguageLayout { get; set; }
		protected LinearLayout InterestLayout { get; set; }
		protected LinearLayout FoodhabitLayout { get; set; }
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.ProfileView"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="user">User.</param>
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

			Languages = new List<Language>();
			Interests = new List<Interest>();
			FoodHabits = new List<FoodHabit>();

			PictureView = new ImageView(context);
			NameView = new TextView(context);
			AddressView = new TextView(context);

			LanguageLayout = new LinearLayout(context);
			InterestLayout = new LinearLayout(context);
			FoodhabitLayout = new LinearLayout(context);
			#endregion

			#region Setting variables values
			Orientation = Orientation.Vertical;
			infolayout.Orientation = Orientation.Vertical;
			infolayout.SetHorizontalGravity(GravityFlags.Right);
			LanguageLayout.Orientation = Orientation.Vertical;
			InterestLayout.Orientation = Orientation.Vertical;
			FoodhabitLayout.Orientation = Orientation.Vertical;
			detailLayout.SetBackgroundColor(Color.Orange);

			NameView.Text = "Name: " + User.Name;
			AddressView.Text = "Address: " + User.Address;
			languagetext.Text = "Languages";
			intereststext.Text = "\nInterests";
			foodhabitstext.Text = "\nFood Habits";
			editdetails.Text = "Edit";
			editlanguage.Text = "Edit";
			editinterests.Text = "Edit";
			editfoodhabits.Text = "Edit";

			languagetext.SetTypeface(null, TypefaceStyle.Bold);
			intereststext.SetTypeface(null, TypefaceStyle.Bold);
			foodhabitstext.SetTypeface(null, TypefaceStyle.Bold);

			PictureView.SetImageResource(Resource.Drawable.Icon);
			PictureView.SetMinimumHeight(100);
			PictureView.SetMinimumWidth(100);

			PictureView.Id = 5;
			NameView.Id = 6;
			AddressView.Id = 7;

			editdetails.Click += EditDetails;
			editlanguage.Click += (sender, e) => 
				{
					SetupEditDialog(InfoType.Language, Languages, LanguageLayout);
				};
			editinterests.Click += (sender, e) => 
				{
					SetupEditDialog(InfoType.Interest, Interests, InterestLayout);
				};
			editfoodhabits.Click += (sender, e) => 
				{
					SetupEditDialog(InfoType.FoodHabit, FoodHabits, FoodhabitLayout);
				};
			#endregion

			#region Adding views to layouts
			detailLayout.AddView(PictureView);
			detailLayout.AddView(NameView);
			detailLayout.AddView(AddressView);
			detailLayout.AddView(editdetails);

			infolayout.AddView(languagetext);
			infolayout.AddView(new Seperator(context));
			infolayout.AddView(LanguageLayout);
			infolayout.AddView(editlanguage);

			infolayout.AddView(intereststext);
			infolayout.AddView(new Seperator(context));
			infolayout.AddView(InterestLayout);
			infolayout.AddView(editinterests);

			infolayout.AddView(foodhabitstext);
			infolayout.AddView(new Seperator(context));
			infolayout.AddView(FoodhabitLayout);
			infolayout.AddView(editfoodhabits);

			scrollview.AddView(infolayout);

			AddView(detailLayout);
			AddView(new Seperator(context));
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

		#region Private Methods
		private async void GetInfo()
		{
			throw new NotImplementedException();
		}

		private void UpdateLayout(InfoType type, LinearLayout layout, IEnumerable items)
		{
			string[] names;
		
			layout.RemoveAllViews();

			foreach (var item in items)
			{
				var view = new TextView(Context);
				view.Text = Names[(int)type][(int)item];
				layout.AddView(view);
			}
		}

		private void EditDetails(object sender, EventArgs evntarg)
		{
			var dialog = new Dialog(Context);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditDetails);

			// getting relevant views
			var nameview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.NameEditor);
			var addressview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.AddressEditor);
			var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
			var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

			// setting textbox info
			nameview.Text = User.Name;
			addressview.Text = User.Address;

			// adding functionallity to save button
			savebutton.Click += (s, e) => 
				{
					// updating user name and address
					User.Name = nameview.Text;
					User.Address = addressview.Text;

					// updating ui
					NameView.Text = "Name: " + User.Name;
					AddressView.Text = "Address: " + User.Address;

					// closing dialog
					dialog.Dismiss();
				};

			// adding functionallity to cancelbutton
			cancelbutton.Click += (s, e) => 
				{
					// closing dialog
					dialog.Dismiss();
				};

			// show dialog
			dialog.Show();
   		}

		private void SetupEditDialog(InfoType type, IList info, LinearLayout layout)
		{
			var dialog = new Dialog(Context);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditComparableInfo);

			// getting relevant views
			var nameview = dialog.FindViewById<TextView>(Resource.Id.Text);
			var infolist = dialog.FindViewById<ListView>(Resource.Id.InfoList);
			var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
			var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

			// setting header text info
			nameview.Text = Titles[(int)type];

			// setting up list of info with checkboxes
			infolist.Adapter = new ArrayAdapter<string>(Context, Resource.Layout.CheckedListViewItem, Names[(int)type]);
			infolist.ChoiceMode = ChoiceMode.Multiple;

			// selecting info that is already chosen by user
			foreach (var item in info)
				infolist.SetItemChecked((int)item, true);

			// adding functionallity to save button
			savebutton.Click += (s, e) => 
				{
					info.Clear();

					// adding selected info to list
					for (int i = 0; i < infolist.ChildCount; i++) 
						if ((infolist.GetChildAt(i) as CheckBox).Checked)
							info.Add(i);

					// updating ui
					UpdateLayout(type, layout, info);

					// closing dialog
					dialog.Dismiss();
				};

			// adding functionallity to cancelbutton
			cancelbutton.Click += (s, e) => 
				{
					// closing dialog
					dialog.Dismiss();
				};
			
			dialog.Show();
		}

		#endregion
	}
}

