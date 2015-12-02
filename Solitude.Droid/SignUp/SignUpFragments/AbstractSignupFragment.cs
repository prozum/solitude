using System;
using Android.OS;
using System.Linq;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	public class AbstractSignupFragment : Android.Support.V4.App.Fragment
	{
		protected View signUpCard { get; set; }	
		protected List<string> signUpInfo = new List<string>();
		protected List<int>[] Info { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			signUpCard = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			foreach (var item in new List<View>() {signUpCard})
			{
				//Sets the input box to visible
				item.FindViewById<ImageView>(Resource.Id.confirm_input).Visibility = ViewStates.Visible;
				item.FindViewById<TextInputLayout>(Resource.Id.info_input_container).Visibility = ViewStates.Visible;

				// Get each entry in the card, and toggle the remove button.
				var content = item.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
				var childCount = content.ChildCount;

				for (int i = 0; i < childCount; i++)
				{
					var entry = content.GetChildAt(i);
					var icon = entry.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
				}
			}
			// Create your fragment here
		}

		public void CreateCard(InfoType type, View card, string subtitle)
		{

			var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
			var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
			var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);

			var autocompleter = card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);
            autocompleter.Click += (o, e) => autocompleter.ShowDropDown();
            //autocompleter.ItemSelected += (o, e) => autocompleter.ShowDropDown();
			autocompleter.SetTokenizer(new Classes.SpaceTokenizer()); // Tells the tokenizer to treat each word as its own entry
			var autocompleteElements = MainActivity.InfoNames[(int)type]; // Gets possible elements to autocomplete to
			var adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleDropDownItem1Line, autocompleteElements);
			autocompleter.Adapter = adapter;

			var adder = card.FindViewById<ImageView>(Resource.Id.confirm_input);
			adder.Click += (o, e) =>
				{
					var input = autocompleter.Text.ToLower();
					autocompleter.Text = string.Empty;
					var compares = MainActivity.InfoNames[(int)type].ToArray(); // Gets an array of all possible entries to compare input with
					foreach (var item in compares)
					{
						if (input.Contains(item.ToLower()))
						{
							AddCardEntry(card, content, item);
						}
					}
				};
			cardTitle.Text = MainActivity.InfoTitles[(int)type];
			cardSubtitle.Text = subtitle;
		}

		void AddCardEntry(View card, LinearLayout content, string s)
		{
			var contentCard = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

			var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
			var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

			remover.Click += (se, ev) =>
				((ViewGroup)contentCard.Parent).RemoveView(contentCard);

			entry.Text = s;
			s = s.Trim(' ');
			signUpInfo.Add(s);
			remover.Visibility = ViewStates.Visible;

			content.AddView(contentCard);
		}
	}
}

