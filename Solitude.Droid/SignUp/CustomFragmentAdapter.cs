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
				return 5;
			}
		}

		public override Android.Support.V4.App.Fragment GetItem (int position)
		{
			switch (SignUpActivity._viewPager.CurrentItem)
			{
				case 0:
					return new SignUpFragmentNameAddress();
				case 1:
					return new SignUpFragmentBirthdate();
				case 2:
					return new SignUpFragmentUsernamePassword();
				case 3:
					return new SignUpFragmentInterests();
				case 4:
					return new SignUpFragmentFoodPreferences();
				default:
					return new SignUpFragmentNameAddress();
			}
		}
	}
}