using Android.Support.V4.App;
using Android.Widget;
using Android.Views.InputMethods;

using Android.App;
using Android.Content;

namespace Solitude.Droid 
{
	public class CustomFragmentAdapter: FragmentPagerAdapter
	{
		public enum CurrentlyShown
		{
			NameAddress, Interests, FoodPreferences, Languages, UsernamePassword
		};
		private readonly int NUMBER_OF_PAGES = 5;
		SignUpFragmentNameAddress nameAdd;
		SignUpFragmentUsernamePassword usPa;
		SignUpFragmentInterests ints;
		SignUpFragmentLanguages lang;
		SignUpFragmentFoodPreferences fp;

		public Android.Support.V4.App.Fragment CurrentItem 
		{
			get { return null/*GetItem(SignUpActivity._viewPager.CurrentItem)*/; }
		}
		public CustomFragmentAdapter (Android.Support.V4.App.FragmentManager fm) : base (fm)
		{ 
			nameAdd = new SignUpFragmentNameAddress();
			usPa = new SignUpFragmentUsernamePassword();
			ints = new SignUpFragmentInterests();
			lang = new SignUpFragmentLanguages();
			fp = new SignUpFragmentFoodPreferences();
		}

		public override int Count {
			get 
			{
				return NUMBER_OF_PAGES;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			switch ((CurrentlyShown) position)
			{
				case CurrentlyShown.NameAddress:
					return nameAdd;
				case CurrentlyShown.Interests:
					return ints;
				case CurrentlyShown.FoodPreferences:
					return fp;
				case CurrentlyShown.Languages:
					return lang;
				case CurrentlyShown.UsernamePassword:
					return usPa;
				default:
					return null;
			}
		}
	}
}