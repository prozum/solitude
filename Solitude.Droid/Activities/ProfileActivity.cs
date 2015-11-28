using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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
using Java.Lang;
using Android.Text;
using Android.Support.V7.Widget;

namespace Solitude.Droid
{
	[Activity(Label = "@string/label_profileactivity", Icon = "@drawable/Profile_Icon")]
	public class ProfileActivity : DrawerActivity
	{
		protected User User { get; set; }

		protected List<int>[] Info { get; set; }

        bool isEditing = false;
        LinearLayout profileContent;

        View cardFood;
        View cardInterest;
        View cardLanguage;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			///var profile = new ProfileView(this, new User("Jimmi", "Jimmivej 12"));

			// setting up drawer
			base.OnCreate(savedInstanceState);

			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					//PrepareLooper();

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
            menu.Add(0, 1, 1, GetString(Resource.String.profile_menu_edit_foodhabit));
            menu.Add(0, 2, 2, GetString(Resource.String.profile_menu_edit_interests));
            menu.Add(0, 3, 3, GetString(Resource.String.profile_menu_edit_language));
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

            cardFood = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
            cardInterest = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
            cardLanguage = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);

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

            ProfileCreateCard(InfoType.Language, cardLanguage, "Speaks");
            ProfileCreateCard(InfoType.Interest, cardInterest, "Likes");
            ProfileCreateCard(InfoType.FoodHabit, cardFood, "Prefers");
        }

        private void ProfileCreateCard(InfoType type, View card, string subtitle)
        {
            var info = MainActivity.CIF.GetInformation();
            
            var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
            var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
            var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
            
            var autocompleter = card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);
            autocompleter.SetTokenizer(new Classes.SpaceTokenizer());
            var autocompleteElements = MainActivity.InfoNames[(int)type];
            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, autocompleteElements);
            autocompleter.Adapter = adapter;

            var adder = card.FindViewById<ImageView>(Resource.Id.confirm_input);
            adder.Click += (o, e) =>
            {
                var input = autocompleter.Text.Split(' ');
                autocompleter.Text = string.Empty;
                var compares = MainActivity.InfoNames[(int)type].Select(s => s.ToLower()).ToArray();
                
                foreach (var item in input)
                {
                    if (compares.Contains(item.ToLower()))
                    {
                        AddCardEntry(content, item);
                    }
                }
            };

            cardTitle.Text = MainActivity.InfoTitles[(int)type];
            cardSubtitle.Text = subtitle;

#if DEBUG
            foreach (var item in new List<string>() { "something", "nothing", "everything" })
#else
            foreach (var item in info[(int)type])
#endif
            {
                AddCardEntry(content, item);
            }

            profileContent.AddView(card);
        }

        private void AddCardEntry(LinearLayout content, string s)
        {
            var contentCard = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

            var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
            var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

            remover.Click += (se, ev) =>
            {
                ((ViewGroup)contentCard.Parent).RemoveView(contentCard);
            };

            entry.Text = s;
            remover.Visibility = isEditing ? ViewStates.Visible : ViewStates.Invisible;

            content.AddView(contentCard);
        }

        private void EditProfile(object sender, EventArgs e)
        {
            var edit = FindViewById<FloatingActionButton>(Resource.Id.fab_edit_profile);

            if (isEditing)
            {
                edit.SetImageResource(Resource.Drawable.ic_mode_edit_white_48dp);
                SetContentVisible(ViewStates.Invisible);
            }
            else
            {
                edit.SetImageResource(Resource.Drawable.ic_done_white_48dp);
                SetContentVisible(ViewStates.Visible);
            }

            isEditing = !isEditing;
        }

        private void SetContentVisible(ViewStates state)
        {
            foreach (var item in new List<View>() { cardFood, cardInterest, cardLanguage })
            {
                item.FindViewById<ImageView>(Resource.Id.confirm_input).Visibility = state.Equals(ViewStates.Visible) ? ViewStates.Visible : ViewStates.Gone;
                item.FindViewById<TextInputLayout>(Resource.Id.info_input_container).Visibility = state.Equals(ViewStates.Visible) ? ViewStates.Visible : ViewStates.Gone;

                var content = item.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
                var childCount = content.ChildCount;
                
                for (int i = 0; i < childCount; i++)
                {
                    var entry = content.GetChildAt(i);
                    var icon = entry.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
                    icon.Visibility = state;
                }
            }
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