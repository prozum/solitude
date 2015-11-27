using Android.Support.V4.App;
using Android.Widget;

namespace Solitude.Droid 
{
	public class CustomFragmentAdapter: FragmentPagerAdapter
	{
		public enum CurrentlyShown
		{
			NameAddress, UsernamePassword, Interests, FoodPreferences, Languages
		};

		SignUpFragmentNameAddress nameAdd;
		SignUpFragmentUsernamePassword usPa;
		SignUpFragmentInterests ints;
		SignUpFragmentLanguages lang;
		SignUpFragmentFoodPreferences fp;

		public Fragment CurrentItem 
		{
			get { return GetItem(SignUpActivity._viewPager.CurrentItem); }
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
				return (int) CurrentlyShown.Languages + 1;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			switch ((CurrentlyShown) position)
			{
				case CurrentlyShown.NameAddress:
					return nameAdd;
				case CurrentlyShown.UsernamePassword:
					return usPa;
				case CurrentlyShown.Interests:
					return ints;
				case CurrentlyShown.FoodPreferences:
					return fp;
				case CurrentlyShown.Languages:
					return lang;
				default:
					return null;
			}
		}
	}
}