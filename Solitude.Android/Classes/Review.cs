using System;
using System.Collections.Generic;
using DineWithaDane.Android;
using System.Threading.Tasks;

namespace DineWithaDane.Android
{
	public class Review
	{		
		User _user;
		List<Review> userReviews;

		public Review (User user)
		{
			_user = user;
		}
	}
}

