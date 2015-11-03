﻿using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TileMenu
{
	public abstract class BaseTileListAdapter<T> : BaseExpandableListAdapter
	{
		#region Fields
		protected Activity Context
		{
			get;
			private set;
		}
		protected List<T> Items
		{
			get;
			private set;
		}

		public override int GroupCount
		{
			get
			{
				return Items.Count;
			}
		}

		public override bool HasStableIds 
		{
			get 
			{
				return true;
			}
		}
		#endregion

		#region Constructors
		public BaseTileListAdapter(Activity context, List<T> items) : base()
		{
			Context = context;
			Items = items;
		}
		#endregion

		#region Public Methods
		public abstract void Sort(string context);

		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = Context.LayoutInflater.Inflate(Resource.Layout.ListGroup, null);

			if (isExpanded)
			{
				view.FindViewById<TextView>(Resource.Id.Dots).Visibility = ViewStates.Gone;
				view.FindViewById<TextView>(Resource.Id.GrayLine1).Visibility = ViewStates.Gone;
			}
			else
			{
				view.FindViewById<TextView>(Resource.Id.Dots).Visibility = ViewStates.Visible;
				view.FindViewById<TextView>(Resource.Id.GrayLine1).Visibility = ViewStates.Visible;
			}


			return view;
		}

		public void RemoveAt(int index)
		{
			Items.RemoveAt(index);
			NotifyDataSetChanged();
		}

		public override long GetGroupId(int groupPosition)
		{
			return groupPosition;
		}

		public override long GetChildId(int groupPosition, int childPosition)
		{
			return groupPosition;
		}

		public override int GetChildrenCount(int groupPosition)
		{
			return 1;
		}

		public override bool IsChildSelectable (int groupPosition, int childPosition)
		{
			return false;
		}

		public override Java.Lang.Object GetGroup(int groupPosition)
		{
			throw new NotImplementedException();
		}

		public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Private Methods
		#endregion
	}
}
