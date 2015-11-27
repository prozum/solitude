using System;
using Android.Support.V4.View;
using Android.Content;
using Android.Util;
using Android.Support.V4.App;
using Android.Widget;

namespace Solitude.Droid
{
	public delegate void OnPageLeftHandler (object sender, FragmentEventArgs e);
	public class FragmentEventArgs
	{
		public Fragment fragment
		{
			get;
			private set;
		}

		public FragmentEventArgs (Fragment f)
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
			if (OnPageLeft != null)
				OnPageLeft(this, new FragmentEventArgs((Adapter as CustomFragmentAdapter).GetItem(CurrentItem)));
			
			base.OnPageScrolled(position, offset, offsetPixels);
		}

		/*public override bool OnTouchEvent (Android.Views.MotionEvent e)
		{
			return this.isPagingEnabled && base.OnTouchEvent (e);
		}

		public override bool OnInterceptTouchEvent (Android.Views.MotionEvent ev)
		{
			return this.isPagingEnabled && base.OnInterceptTouchEvent (ev);
		}

		public void setPagingEnabled(bool b) {
			this.isPagingEnabled = b;
		}*/
	}
}

