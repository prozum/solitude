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
		protected List<int> signUpInfo = new List<int>();
		protected List<int>[] Info { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			signUpCard = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			signUpCard.FindViewById<ImageView>(Resource.Id.confirm_input).Visibility = ViewStates.Visible;
			signUpCard.FindViewById<TextInputLayout>(Resource.Id.info_input_container).Visibility = ViewStates.Visible;
			// Create your fragment here
		}

		public void CreateCard(InfoType type, View card, string subtitle)
		{
			var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
			var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
			var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);

			var autocompleter = card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);
            autocompleter.Click += (o, e) => autocompleter.ShowDropDown();
            autocompleter.Threshold = 0;
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
							AddCardEntry(card, content, item, type);
						}
					}
				};
			cardTitle.Text = MainActivity.InfoTitles[(int)type];
			cardSubtitle.Text = subtitle;
		}

		void AddCardEntry(View card, LinearLayout content, string s, InfoType type)
		{
			var contentCard = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

			var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
			var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

			remover.Click += (se, ev) =>
				((ViewGroup)contentCard.Parent).RemoveView(contentCard);

			entry.Text = s;
			var info = Array.IndexOf(MainActivity.InfoNames[(int)type], s);
			signUpInfo.Add(info);
			remover.Visibility = ViewStates.Visible;

			content.AddView(contentCard);
		}
	}
}

