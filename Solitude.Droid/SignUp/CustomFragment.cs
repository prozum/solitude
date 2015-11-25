using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.OS;

namespace Solitude.Droid
{
	public class CustomFragment : Fragment
	{
		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var view = View;

			switch (SignUpActivity._viewPager.CurrentItem)
			{
				case 0:
					view = inflater.Inflate(Resource.Layout.signupFragLayout1, container, false);
					break;
				case 1:
					view = inflater.Inflate(Resource.Layout.signupFragLayout2, container, false);
					break;
				case 2:
					view = inflater.Inflate(Resource.Layout.signupFragLayout3, container, false);
					break;
				case 3:
					view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
					break;
				case 4:
					view = inflater.Inflate(Resource.Layout.signupFragLayout5, container, false);
					break;
				//default:
				//	view = inflater.Inflate(Resource.Layout.signupFragLayout1, container, false);
				//	break;
			}
	
			return view;
		}
	}
}