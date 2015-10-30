using System;
using System.Collections.Generic;

namespace TileMenu
{
	public class DistanceComparer : IComparer<Event>
	{
		public int Compare(Event evnt1, Event evnt2)
		{
			return evnt1.Place.CompareTo(evnt2.Place);
		}
	}

	public class TitleComparer : IComparer<Event>
	{
		public int Compare(Event evnt1, Event evnt2)
		{
			return evnt1.Title.CompareTo(evnt2.Title);
		}
	}

	public class DateComparer : IComparer<Event>
	{
		public int Compare(Event evnt1, Event evnt2)
		{
			return evnt1.Date.CompareTo(evnt2.Date);
		}
	}

}

