using System;

namespace Solitude.Droid
{
	public class Offer : Event
	{
		public Match Match { get; set; } 

		public Offer()
		{
			
		}

		public Offer (string title, DateTime date, string place, string desc, int max, int left)
			:base(title, date, place, desc, max, left)
		{
		}

		public Offer (string title, DateTime date, string place, string desc, int max, int left, int ID)
			:base(title, date, place, desc, max, left, ID)
		{
		}
	}
}

