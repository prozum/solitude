using System;
using Android.Support.V4.View;
using Android.Content;
using Android.Util;
using Android.Support.V4.App;
using Android.Widget;
using Android.Views.InputMethods;
using Android.App;

namespace Solitude.Droid
{
	public delegate void OnPageLeftHandler (object sender, FragmentEventArgs e);
	public class FragmentEventArgs
	{
		public Android.Support.V4.App.Fragment fragment
		{
			get;
			private set;
		}

		public FragmentEventArgs (Android.Support.V4.App.Fragment f)
		{
			this.fragment = f;
		}
	}

	public class CustomViewPager : ViewPager
	{
		public bool ScrollingEnabled
		{
			private get;
			set;
		}


		public event OnPageLeftHandler OnPageLeft;

		public CustomViewPager(Context context) : base (context){

		}

		public CustomViewPager (Context context, IAttributeSet attrs) : base (context, attrs){

		}

		protected override void OnPageScrolled(int position, float offset, int offsetPixels)
		{
			if (OnPageLeft != null && offset != 0 && offsetPixels != 0)
				OnPageLeft(this, new FragmentEventArgs((Adapter as CustomFragmentAdapter).GetItem(CurrentItem)));
			
			base.OnPageScrolled(position, offset, offsetPixels);
		}
	}
}

