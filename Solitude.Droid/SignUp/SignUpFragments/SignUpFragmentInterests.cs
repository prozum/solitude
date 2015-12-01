
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
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	public class SignUpFragmentInterests : Android.Support.V4.App.Fragment
	{
		//ListView interestListView;
		protected List<int>[] Info { get; set; }
		List<bool> interestList = new List<bool>();
		protected View cardInterest { get; set; }
		protected bool isEditing { get; set; }
		/*public List<int> Interests
		{
			get
			{
				for (int i = 0; i < 2; i++)
					//if ((interestListView.GetChildAt(i) as CheckBox).Checked)
						Interests.Add(i);

				return _interests;
			}
		}*/
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			cardInterest = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			foreach (var item in new List<View>() {cardInterest})
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
					icon.Visibility = ViewStates.Invisible;
				}
			}
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			#region Checkbox
			var userInterests = new List<int>();

			//Inflate view and find content
			//View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			/*
			interestListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			var desc = view.FindViewById<TextView>(Resource.Id.signupListDescription);

			//Adds the description
			desc.Text = GetString(Resource.String.profile_menu_edit_interests);

			//Populate ListView
			interestListView.Adapter = new ArrayAdapter<string>(Activity, 
				Resource.Layout.CheckedListViewItem, MainActivity.InfoNames[(int)InfoType.Interest]);
			interestListView.ChoiceMode = ChoiceMode.Multiple; */
			#endregion

			CreateCard(InfoType.Interest, cardInterest, "Fisk");

			return cardInterest;
		}
		public void CreateCard(InfoType type, View card, string subtitle)
		{

			var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
			var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
			var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);

			var autocompleter = card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);
			autocompleter.SetTokenizer(new Classes.SpaceTokenizer()); // Tells the tokenizer to treat each word as its own entry
			var autocompleteElements = MainActivity.InfoNames[(int)type]; // Gets possible elemnts to autocomplete to
			var adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleDropDownItem1Line, autocompleteElements);
			autocompleter.Adapter = adapter;

			var adder = card.FindViewById<ImageView>(Resource.Id.confirm_input);
			adder.Click += (o, e) =>
			{
				var input = autocompleter.Text.Split(' ');
				autocompleter.Text = string.Empty;
				var compares = MainActivity.InfoNames[(int)type].Select(s => s.ToLower()).ToArray(); // Gets an array of all possible entries to compare input with
				foreach (var item in input)
				{
					if (compares.Contains(item.ToLower())) // Checks if entry is valid
					{
						AddCardEntry(card, content, item);
					}
				}
			};
			cardTitle.Text = MainActivity.InfoTitles[(int)type];
			cardSubtitle.Text = subtitle;
		}


		private void AddCardEntry(View card, LinearLayout content, string s)
		{
			var contentCard = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

			var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
			var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

			remover.Click += (se, ev) =>
				{
					((ViewGroup)contentCard.Parent).RemoveView(contentCard); // Removes entry from card when clicked
				};

			entry.Text = s;
			remover.Visibility = ViewStates.Visible;

			content.AddView(contentCard);
		}


		private void SaveInfo(InfoType type, List<int> changes)
		{
			List<int> info = Info[(int)type];

			for (int i = 0; i < info.Count;)
			{
				if (!changes.Contains(info[i]))
				{
					//info.Remove(info[i]);
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
					//MainActivity.CIF.AddInformation(new InfoChange(type, item));
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

				contentList.Add(Array.FindIndex(MainActivity.InfoNames[(int)type], t => t.IndexOf(text.Text, StringComparison.InvariantCultureIgnoreCase) >= 0));
			}

			return contentList;
		}//REMOVE THIS
	}
}

