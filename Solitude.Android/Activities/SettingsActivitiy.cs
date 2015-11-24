
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Collections;

namespace DineWithaDane.Droid
{
	[Activity(Label = "Settings", Icon = "@drawable/Settings_Icon")]			
	public class SettingsActivitiy : DrawerActivity
	{
		public static readonly string[] Titles = new string[]
		{
			"Languages",
			"Interests",
			"Food Habits"
		};

		public static readonly string[][] Names = new string[][]
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
				"Lactose Intolerance",
				"Gluten Intolerance",
				"Nut Allergy"
			}
		};

		TextView buttonDeleteAccount;
		TextView buttonChangeLanguage;
		TextView buttonChangeFoodHabits;
		TextView buttonChangeInterests;

		protected List<int>[] Info { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			// Layout setup
			var settingsLayout = new LinearLayout(this);
			settingsLayout.Orientation = Orientation.Vertical;

			Info = MainActivity.CIF.GetInformation();

			/*
			// Button setup
			var deletbutton = new Button(this);
			deletbutton.Text = "Delete Account";
			deletbutton.Click += (sender, e) => deletionDialog();

			settingsLayout.AddView(deletbutton);
			*/

			buttonChangeLanguage = new TextView(this);
			buttonChangeLanguage.Gravity = GravityFlags.Center;
			buttonChangeLanguage.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			buttonChangeLanguage.Text = "\nEdit Languages\n";
			buttonChangeLanguage.TextSize = 24;

			buttonChangeLanguage.Click += (object sender, EventArgs e) =>
			{
				buttonChangeLanguage.SetBackgroundColor(Color.Orange);
				SetupEditDialog(InfoType.Language, Info[(int)InfoType.Language]);
			};

			buttonChangeFoodHabits = new TextView(this);
			buttonChangeFoodHabits.Gravity = GravityFlags.Center;
			buttonChangeFoodHabits.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			buttonChangeFoodHabits.Text = "\nEdit Food Preferences\n";
			buttonChangeFoodHabits.TextSize = 24;

			buttonChangeFoodHabits.Click += (object sender, EventArgs e) =>
			{
				buttonChangeFoodHabits.SetBackgroundColor(Color.Orange);
				SetupEditDialog(InfoType.FoodHabit, Info[(int)InfoType.FoodHabit]);
			};

			buttonChangeInterests = new TextView(this);
			buttonChangeInterests.Gravity = GravityFlags.Center;
			buttonChangeInterests.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			buttonChangeInterests.Text = "\nEdit Interests\n";
			buttonChangeInterests.TextSize = 24;

			buttonChangeInterests.Click += (object sender, EventArgs e) =>
			{
				buttonChangeInterests.SetBackgroundColor(Color.Orange);
				SetupEditDialog(InfoType.Interest, Info[(int)InfoType.Interest]);
			};

			buttonDeleteAccount = new TextView(this);
			buttonDeleteAccount.Gravity = GravityFlags.Center;
			buttonDeleteAccount.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			buttonDeleteAccount.Text = "\nDelete Account\n";
			buttonDeleteAccount.TextSize = 24;

			buttonDeleteAccount.Click += (object sender, EventArgs e) =>
			{
				buttonDeleteAccount.SetBackgroundColor(Color.Orange);
				deletionDialog();
			};

			base.OnCreate(bundle);

			settingsLayout.AddView(buttonChangeLanguage);
			settingsLayout.AddView(new Seperator(this));
			settingsLayout.AddView(buttonChangeFoodHabits);
			settingsLayout.AddView(new Seperator(this));
			settingsLayout.AddView(buttonChangeInterests);
			settingsLayout.AddView(new Seperator(this));
			settingsLayout.AddView(buttonDeleteAccount);
			settingsLayout.AddView(new Seperator(this));

			Content.AddView(settingsLayout);
		}

		private void deletionDialog()
		{
			var dialog = new Dialog(this);
			Button cancelButton, acceptButton;
	
			dialog.SetContentView(Resource.Layout.deletionDialog);
			dialog.SetTitle("Are you sure?");

			cancelButton = (Button)dialog.FindViewById <Button>(Resource.Id.CancelDeletionBtn);
			acceptButton = (Button)dialog.FindViewById <Button>(Resource.Id.deleteAccountBtn);

			cancelButton.Click += (sender, e) =>
			{
				dialog.Dismiss();
				buttonDeleteAccount.SetBackgroundColor(Color.Transparent);
			};

			acceptButton.Click += (sender, e) =>
			{
				MainActivity.CIF.DeleteUser();
				MainActivity.CIF.Logout(this);
				buttonDeleteAccount.SetBackgroundColor(Color.Transparent);
			};

			dialog.Show();
		}

		private void SetupEditDialog(InfoType type, List<int> info)
		{
			var dialog = new InfoDialog(this, type, info);

			// adding functionallity to save button
			dialog.SaveButton.Click += (s, e) =>
			{
				UpdateInfo(type, dialog.ItemsChecked());

				// closing dialog
				dialog.Dismiss();

				buttonChangeFoodHabits.SetBackgroundColor(Color.Transparent);
				buttonChangeLanguage.SetBackgroundColor(Color.Transparent);
				buttonChangeInterests.SetBackgroundColor(Color.Transparent);
			};

			// adding functionallity to cancelbutton
			dialog.CancelButton.Click += (s, e) =>
			{
				// closing dialog
				dialog.Dismiss();

				buttonChangeFoodHabits.SetBackgroundColor(Color.Transparent);
				buttonChangeLanguage.SetBackgroundColor(Color.Transparent);
				buttonChangeInterests.SetBackgroundColor(Color.Transparent);
			};

			dialog.Show();
		}

		private void UpdateInfo(InfoType type, List<int> changes)
		{
			var info = Info[(int)type];

			//foreach (var item in info[(int)type])
			for (int i = 0; i < info.Count;)
			{
				if (!changes.Contains(info[i]))
				{
					MainActivity.CIF.DeleteInformation(new InfoChange(type, info[i]));
					info.Remove(info[i]);
				}
				else
				{
					i++;
				}
			}

			foreach (var item in changes)
			{
				if (!info.Contains(item))
				{
					MainActivity.CIF.AddInformation(new InfoChange(type, item));
					info.Add(item);
				}
			}
		}
	}
}

