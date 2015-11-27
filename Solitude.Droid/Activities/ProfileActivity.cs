using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Graphics;
using Android.Content.PM;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	[Activity(Label = "@string/label_profileactivity", Icon = "@drawable/Profile_Icon")]
	public class ProfileActivity : DrawerActivity
	{
		protected User User { get; set; }

		protected List<int>[] Info { get; set; }

        bool isEditing = false;
        LinearLayout profileContent;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			///var profile = new ProfileView(this, new User("Jimmi", "Jimmivej 12"));

			// setting up drawer
			base.OnCreate(savedInstanceState);

			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();

					Info = MainActivity.CIF.GetInformation();
					User = MainActivity.CIF.GetUserData();

					//Clear screen and show the found offers
					RunOnUiThread(() =>
						{
							ClearLayout();
							SetupUI();
						});
				});

		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			base.OnCreateOptionsMenu(menu);
			menu.Add(0,1,1,GetString(Resource.String.profile_menu_edit_foodhabit));
			menu.Add(0,2,2,GetString(Resource.String.profile_menu_edit_interests));
			menu.Add(0,3,3,GetString(Resource.String.profile_menu_edit_language));
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case 1:
					SetupEditDialog(InfoType.FoodHabit, Info[(int)InfoType.FoodHabit]);
					break;
				case 2:
					SetupEditDialog(InfoType.Interest, Info[(int)InfoType.Interest]);
					break;
				case 3:
					SetupEditDialog(InfoType.Language, Info[(int)InfoType.Language]);
					break;
				default:
					base.OnOptionsItemSelected(item);
					break;
			}
			return true;
		}
		private void SetupUI()
		{
			// add profile to activity
			var profile = LayoutInflater.Inflate(Resource.Layout.Profile, null);
			Content.AddView(profile);

			var picture = profile.FindViewById<ImageView>(Resource.Id.Image);
			var name = profile.FindViewById<TextView>(Resource.Id.Name);
			var address = profile.FindViewById<TextView>(Resource.Id.Address);
			var age = profile.FindViewById<TextView>(Resource.Id.Age);
			var layout = profile.FindViewById<LinearLayout>(Resource.Id.Layout);

            var edit = profile.FindViewById<FloatingActionButton>(Resource.Id.fab_edit_profile);

            profileContent = profile.FindViewById<LinearLayout>(Resource.Id.profile_content);

			//var adapter = new InfoAdapter(this, Info);
			//var tilemenu = new InfoList(this, adapter);

			//layout.AddView(tilemenu);

			name.SetTypeface(null, TypefaceStyle.Bold);
			name.TextSize = 20;
			name.Text = User.Name;

			address.Text = User.Address;
			DateTime today = DateTime.Today;
			int iAge = today.Year - User.Birthdate.Year;
			if (User.Birthdate > today.AddYears(-iAge))
				iAge--;
			age.Text = iAge + Resources.GetString(Resource.String.year_old);

            edit.Click += EditProfile;

            ProfileCard(InfoType.Language, "Speaks");
            ProfileCard(InfoType.Interest, "Likes");
            ProfileCard(InfoType.FoodHabit, "Prefers");
		}

        private void ProfileCard(InfoType type, string subtitle)
        {
            var info = MainActivity.CIF.GetInformation();

            var card = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
            var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
            var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
            var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_content);

            cardTitle.Text = MainActivity.InfoTitles[(int)type];
            cardSubtitle.Text = subtitle;

            foreach (var item in info[(int)type])
            {
                var entry = new TextView(this);
                entry.Text = MainActivity.InfoNames[(int)type][item];
                entry.SetPaddingRelative(16, 8, 16, 8);
                content.AddView(entry);
            }
            profileContent.AddView(card);
        }

        private void EditProfile(object sender, EventArgs e)
        {
            var edit = FindViewById<FloatingActionButton>(Resource.Id.fab_edit_profile);

            if(isEditing)
            {
                edit.SetImageResource(Resource.Drawable.ic_mode_edit_white_48dp);
            }
            else
            {
                edit.SetImageResource(Resource.Drawable.ic_done_white_48dp);
            }

            isEditing = !isEditing;
        }

        void SetupEditDialog(InfoType type, List<int> info)
		{
			var dialog = new InfoDialog(this, type, info);

			// adding functionallity to save button
			dialog.SaveButton.Click += (s, e) =>
				{
					UpdateInfo(type, dialog.ItemsChecked());

					// closing dialog
					dialog.Dismiss();
				};

			// adding functionallity to cancelbutton
			dialog.CancelButton.Click += (s, e) =>
				{
					// closing dialog
					dialog.Dismiss();
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