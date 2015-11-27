using System;

namespace Model
{
	public class Review
	{
		public Guid Id { set; get;}
		public string Text { set; get; }
		public int Rating { set; get; }
		public Guid UserId { get; set; }
		public Guid EventId { get; set; }
	}
}

