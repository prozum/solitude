using System;

namespace BBBClasses
{
	public class Review
	{
		public Guid Id { set; get;}
		public Guid UserId { get; set; }
		public Guid BeerId { get; set; }

		public string Text { set; get; }
		public int MouthfeelRating { set; get; }
		public int LookRating { get; set; }
		public int TasteRating { get; set; }
		public int AromaRating { get; set; }
	}
}

