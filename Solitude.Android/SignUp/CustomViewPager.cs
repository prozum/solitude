﻿using System;
using Android.Support.V4.View;
using Android.Content;
using Android.Util;

namespace Solitude.Droid
{
	public class CustomViewPager : ViewPager
	{
		private bool isPagingEnabled = false;

		public CustomViewPager(Context context) : base (context){

		}

		public CustomViewPager (Context context, IAttributeSet attrs) : base (context, attrs){

		}

		public override bool OnTouchEvent (Android.Views.MotionEvent e)
		{
			return this.isPagingEnabled && base.OnTouchEvent (e);
		}

		public override bool OnInterceptTouchEvent (Android.Views.MotionEvent ev)
		{
			return this.isPagingEnabled && base.OnInterceptTouchEvent (ev);
		}

		public void setPagingEnabled(bool b) {
			this.isPagingEnabled = b;
		}
	}
}

