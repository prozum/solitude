using System;

namespace Model
{
	public class Review
	{
		public int Id { set; get;}
		public string Text { set; get; }
		public int Rating { set; get; }
		public string UserId { get; set; }
		public int EventId { get; set; }
	}
}

