﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Android
{
	public class EventTest
	{
		#region Field
		public string Title 
		{
			get;
			set;
		}

		public DateTime Date 
		{
			get;
			set;
		}

		public string Place 
		{
			get;
			set;
		}

		public string Description 
		{
			get;
			set;
		}
		#endregion

		#region Constructor
		public EventTest (string title, DateTime date, string place, string desc)
		{
			Title = title;
			Date = date;
			Place = place;
			Description = desc;
		}
		#endregion

		#region Public Methods
		#endregion

		#region Privat Methods

		#endregion
	}
}

