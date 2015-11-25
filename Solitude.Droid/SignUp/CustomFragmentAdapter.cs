using Android.Support.V4.App;

namespace Solitude.Droid 
{
	public class CustomFragmentAdapter: FragmentPagerAdapter
	{
		public CustomFragmentAdapter (Android.Support.V4.App.FragmentManager fm) : base (fm)
		{ 
				
		}

		public override int Count {
			get 
			{
				return 12;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			switch (SignUpActivity._viewPager.CurrentItem)
			{
				case 0:
					return new SignUpFragmentNameAddress();
				case 2:
					return new SignUpFragmentBirthdate();
				case 4:
					return new SignUpFragmentUsernamePassword();
				case 6:
					return new SignUpFragmentInterests();
				case 8:
					return new SignUpFragmentFoodPreferences();
				case 10:
					return new SignUpFragmentLanguages();
				default:
					return null;
			}
		}
	}
}