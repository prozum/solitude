using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Solitude.Droid.TileMenu.TabListeners
{
	class CardTabListener : Android.Support.V7.App.ActionBar.ITabListener
	{
		Android.Support.V4.App.Fragment fragment;

		public IntPtr Handle
		{
			get
			{
				throw new NotImplementedException();
			}
		}


		public CardTabListener(AppCompatActivity activity, EventAdapter<Event> adapter)
		{
			/*
			fragment = activity.LayoutInflater.Inflate(Resource.Layout.EventList, null);

			if (fragment == null)
			{
				fragment = Android.Support.V4.App.Fragment.Instantiate(activity, );
				ft.add(android.R.id.content, mFragment, mTag);
			}


			var list = fragment.View.FindViewById<ListView>(Resource.Id.list);
			list.Adapter = adapter;
			*/
        }

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void OnTabReselected(Android.Support.V7.App.ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
		{
        }

		public void OnTabSelected(Android.Support.V7.App.ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
		{
			/*
			if (fragment == null)
			{
				// If not, instantiate and add it to the activity
				fragment = Android.Support.V4.App.Fragment.Instantiate(mActivity, mClass.getName());
				ft.add(android.R.id.content, mFragment, mTag);
			}
			else
			{
				// If it exists, simply attach it in order to show it
				ft.attach(mFragment);
			}
			*/
		}

		public void OnTabUnselected(Android.Support.V7.App.ActionBar.Tab tab, Android.Support.V4.App.FragmentTransaction ft)
		{
			throw new NotImplementedException();
		}
	}
}