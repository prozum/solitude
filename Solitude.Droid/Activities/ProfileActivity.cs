using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views.InputMethods;

namespace Solitude.Droid
{
    [Activity(Label = "@string/label_profileactivity", Icon = "@drawable/Profile_Icon")]
	public class ProfileActivity : DrawerActivity
	{
		protected User User { get; set; }

		protected List<int>[] Info { get; set; }

        protected bool isEditing { get; set; }
        protected LinearLayout profileContent { get; set; }

        protected View cardFood { get; set; }
        protected View cardInterest { get; set; }
        protected View cardLanguage { get; set; }

		protected FloatingActionButton Edit { get; set; }

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
			menu.Add(0, 0, 0, Resource.String.delete_account_button);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case 0:
                    var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
					dialog.SetMessage(Resource.String.delete_account);
					dialog.SetPositiveButton(Resource.String.yes, (s, e) => ConfirmDeletion());
					dialog.SetNegativeButton(Resource.String.no, (s, e) => { });
					dialog.Show();
					break;
				default:
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		/// <summary>
		/// Sets up the UI, and keeps the OnCreate method clean.
		/// </summary>
		private void SetupUI()
		{
            // add profile to activity
            var profile = LayoutInflater.Inflate(Resource.Layout.Profile, null);
            Content.AddView(profile);

            var picture = profile.FindViewById<ImageView>(Resource.Id.Image);
            var name = profile.FindViewById<TextView>(Resource.Id.Name);
            var address = profile.FindViewById<TextView>(Resource.Id.Address);
            var age = profile.FindViewById<TextView>(Resource.Id.Age);

			Edit = profile.FindViewById<FloatingActionButton>(Resource.Id.fab_edit_profile);

            cardFood = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			cardInterest = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
            cardLanguage = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);

            profileContent = profile.FindViewById<LinearLayout>(Resource.Id.profile_content);

            name.SetTypeface(null, TypefaceStyle.Bold);
            name.TextSize = 20;
            name.Text = User.Name;

            address.Text = User.Location;
            DateTime today = DateTime.Today;
            int iAge = DateTimeOffset.Now.Year - User.Birthdate.Year;
            if (User.Birthdate > today.AddYears(-iAge))
                iAge--;
            age.Text = iAge + Resources.GetString(Resource.String.year_old);

			Edit.Click += EditProfile;

            ProfileCreateCard(InfoType.Language, cardLanguage, "Speaks");
            ProfileCreateCard(InfoType.Interest, cardInterest, "Likes");
            ProfileCreateCard(InfoType.FoodHabit, cardFood, "Prefers");
        }

        /// <summary>
        /// Used to create cards for user information
        /// </summary>
        /// <param name="type">The InfoType that the card should represent. Used for gathering info from user, as well as title and content that can be autocompleted to.</param>
        /// <param name="card">The card that content should be added to.</param>
        /// <param name="subtitle">The subtitle of the card, that appears beneath the title, but above the main content of the card.</param>
        private void ProfileCreateCard(InfoType type, View card, string subtitle)
        {
            var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
            var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
            var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
            
            // Comfigure the autocompleter with a tokenizer and word list
            var autocompleter = card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);
            autocompleter.Click += (o, e) => autocompleter.ShowDropDown();
            autocompleter.Threshold = 0;
            autocompleter.SetTokenizer(new Classes.SpaceTokenizer()); // Tells the tokenizer to treat each word as its own entry
			var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)type, 0);
            var elements = Resources.GetStringArray(id); // Gets possible elemnts to autocomplete to
            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, elements);
            autocompleter.Adapter = adapter;

            var adder = card.FindViewById<ImageView>(Resource.Id.confirm_input);
            adder.Click += (o, e) => AdderClick(type, card, content, autocompleter);
			Edit.Click += (o, e) => AdderClick(type, card, content, autocompleter);

			cardTitle.Text = Resources.GetStringArray(Resource.Array.info_titles)[(int)type];
            cardSubtitle.Text = subtitle;
			foreach (var item in Info[(int)type])
			{
				AddCardEntry(type, card, content, elements[item]);
			}

            profileContent.AddView(card);
        }

		private void AdderClick(InfoType type, View card, LinearLayout content, AppCompatMultiAutoCompleteTextView autocompleter)
		{
			var input = autocompleter.Text.ToLower();
			autocompleter.Text = string.Empty;
			var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)type, 0);
			var compares = Resources.GetStringArray(id);

			foreach (var item in compares)
			{
				if (input.Contains(item.ToLower()) && !CardContains(item, content))
				{
					AddCardEntry(type, card, content, item);
				}
			}
			UpdateInfo(type, GetUpdatedContent(type, card));

		}

		private bool CardContains(string s, LinearLayout content)
		{
			for (int i = 0; i < content.ChildCount; i++)
			{
				if (content.GetChildAt(i).FindViewById<TextView>(Resource.Id.profile_card_entry_content).Text == s)
				{
					return true;
				}
			}

			return false;
		}

        /// <summary>
        /// Adds a new entry to the specified layout in the card
        /// </summary>
        /// <param name="content">The layout in the card that the entry should be added to.</param>
        /// <param name="s">The name of the entry. This is what will be displayed to the user.</param>
        private void AddCardEntry(InfoType type, View card, LinearLayout content, string s)
        {

			var contentCard = LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

            var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
            var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

            remover.Click += (se, ev) =>
            {
                ((ViewGroup)contentCard.Parent).RemoveView(contentCard); // Removes entry from card when clicked
                UpdateInfo(type, GetUpdatedContent(type, card));
            };

            entry.Text = s;
            remover.Visibility = isEditing ? ViewStates.Visible : ViewStates.Invisible; // Checks if the remove burron should be shown when added.

            content.AddView(contentCard);

        }

        /// <summary>
        /// Toggles if the profile is in edit or show mode.
        /// </summary>
        /// <param name="sender">sender, not used in method</param>
        /// <param name="e">eventarguments, nto used in method</param>
        private void EditProfile(object sender, EventArgs e)
        {
            var edit = FindViewById<FloatingActionButton>(Resource.Id.fab_edit_profile);
			
			InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
			imm.HideSoftInputFromWindow(Content.WindowToken, 0);

			if (isEditing) // Go from edit to view
            {
                edit.SetImageResource(Resource.Drawable.ic_mode_edit_white_48dp);
                SetContentVisible(ViewStates.Invisible);
            }
            else // Go from view to edit
            {
                edit.SetImageResource(Resource.Drawable.ic_done_white_48dp);
                SetContentVisible(ViewStates.Visible);
            }

            isEditing = !isEditing;
        }

        /// <summary>
        /// Sets if the edit ccontent of the card is visible or not.
        /// </summary>
        /// <param name="state">The ViewState that specifies if things are shown or not.</param>
        private void SetContentVisible(ViewStates state)
        {
            foreach (var item in new List<View>() { cardFood, cardInterest, cardLanguage })
            {
                // When hidded, these are set to gone, otherwise there will be a lot of empty space at the buttom of the card.
                item.FindViewById<ImageView>(Resource.Id.confirm_input).Visibility = state.Equals(ViewStates.Visible) ? ViewStates.Visible : ViewStates.Gone;
                item.FindViewById<TextInputLayout>(Resource.Id.info_input_container).Visibility = state.Equals(ViewStates.Visible) ? ViewStates.Visible : ViewStates.Gone;

                // Get each entry in the card, and toggle the remove button.
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
		
		private void UpdateInfo(InfoType type, List<int> changes)
		{
            List<int> info = Info[(int)type];

            for (int i = 0; i < info.Count;)
			{
				if (!changes.Contains(info[i]))
				{
					MainActivity.CIF.DeleteInformation(new InfoChange(type, info[i], 1));
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
					MainActivity.CIF.AddInformation(new InfoChange(type, item, 1));
					info.Add(item);
				}
			}
		}

        List<int> GetUpdatedContent(InfoType type, View card)
        {
            List<int> contentList = new List<int>();

            var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
            var childCount = content.ChildCount;

            for (int i = 0; i < childCount; i++)
            {
                var entry = content.GetChildAt(i);
                var text = entry.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

				var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)type, 0);
				contentList.Add(Array.FindIndex(Resources.GetStringArray(id), 
											    t => t.IndexOf(text.Text, StringComparison.InvariantCultureIgnoreCase) >= 0));
            }

            return contentList;
        }

		private void ConfirmDeletion()
		{
			var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
			var field = LayoutInflater.Inflate(Resource.Layout.PasswordLayout, null);
			dialog.SetMessage(Resource.String.delete_account_confirm);
			dialog.SetView(field);
			dialog.SetPositiveButton(Resource.String.confirm, (send, earg) =>
			{
				if (MainActivity.CIF.Login(User.UserName, field.FindViewById<EditText>(Resource.Id.edit).Text))
				{
					MainActivity.CIF.DeleteUser();
					StartActivity(new Intent(this, typeof(MainActivity)));
				}
				else
					WrongPassword();
			});
			dialog.SetNegativeButton(Resource.String.cancel_button, (send, earg) => { });
			dialog.Show();
		}

		private void WrongPassword()
		{
			var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
			dialog.SetMessage(Resource.String.password_wrong);
			dialog.SetPositiveButton(Resource.String.ok, (send, earg) => { });
			dialog.Show();
		}
	}
}