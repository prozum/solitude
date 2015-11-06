using System;

namespace Dal
{
	public class Review
	{
		public int ID { set; get;}
		public string ReviewText { set; get; }
		public int Rating { set; get; }
		public string UserID { get; set; }
	}
}

