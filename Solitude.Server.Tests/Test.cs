using NUnit.Framework;
using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solitude.Server.Tests
{
	[TestFixture ()]
	public class Tests : TestMethods
	{

		public Tests ()
		{
		}
		[TestFixtureSetUp()]
		public void StartUp()
		{
			e.Address = "Test Street 101";
			e.Date = DateTime.Now;
			e.Description = "Literally the greatest event ever";
			e.SlotsTaken = 0;
			e.SlotsTotal = 50;
			e.Title = "Best";
		}

		/*[Test()]
		public void TestUsers ()
		{
			for (int i = 0; i < 50; i++) {
				RegisterUser ();
				Login ();
				if (i % 3 == 0) {
					AddEvent ();
				}
				for (int j = 0; j < 2; j++) {
					AddCharacteristica (0, r.Next(0, 5));
					AddCharacteristica (1, r.Next(0, 6));
					AddCharacteristica (2, r.Next(0, 6));
				}
				if (i % 50 == 0 && i != 0)
				{
					GetOffers ();
					//ReplyOffer ();
				}
			}
		}*/

		[Test()]
		public void TestCaseRegisterUser ()
		{
			RegisterUser ();
		}

		[Test()]
		public void TestCaseLogin()
		{
			RegisterUser ();
			Login ();
		}
			
		[Test ()]
		public void TestCaseAddReview ()
		{
			RegisterUser ();
			Login ();
			AddReview ();
		}

		[Test ()]
		public void TestCaseAddLanguage ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (0, 5);
		}

		[Test ()]
		public void TestCaseAddInterest ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (1, 5);
		}
		[Test ()]
		public void TestCaseAddFoodHabit ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (2, 5);
		}

		[Test()]
		public void TestCaseAddEvent()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
		}

		[Test ()]
		public void TestCaseGetUserData()
		{
			RegisterUser ();
			Login ();
			GetUserData ();
		}
			
		[Test()]
		public void TestCaseGetHostingEvent()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
			GetEvent ();
		}

		[Test ()]
		public void TestCaseGetFoodHabit ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (0, 5);
			GetCharacteristica (0, 5);
		}

		[Test ()]
		public void TestCaseGetInterest ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (1, 5);
			GetCharacteristica (1, 5);
		}

		[Test ()]
		public void TestCaseGetLanguage ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (2, 5);
			GetCharacteristica (2, 5);
		}

		[Test ()]
		public void TestCaseGetOffers ()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
		}

		[Test()]
		public void TestCaseAcceptOffer()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
			ReplyOffer (true);
		}

		[Test()]
		public void TestCaseDeclineOffer()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
			ReplyOffer (false);
		}

		[Test()]
		public void TestCaseUpdateEventChangeTitle()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
			UpdateEventChangeTitle ();
		} 

		[Test()]
		public void TestCaseGetAttendingEvents()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
			ReplyOffer (true);
			GetAttendingEvents ();
		}

		[Test ()]
		public void TestCaseDeleteFoodHabit ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (0, 5);
			DeleteCharacteristica (0, 5);
		}

		[Test ()]
		public void TestCaseDeleteInterest ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (1, 5);
			DeleteCharacteristica (1, 5);
		}

		[Test ()]
		public void TestCaseDeleteLanguage ()
		{
			RegisterUser ();
			Login ();
			AddCharacteristica (2, 5);
			DeleteCharacteristica (2, 5);
		}

		[Test()]
		public void TestCaseDeleteEvent()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
			DeleteEvent ();
		}
			
		[Test ()]
		public void TestCaseDeleteUser ()
		{
			RegisterUser ();
			Login ();
			DeleteUser ();
		}

		[Test ()]
		public void TestCaseCancelRegistration ()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
			ReplyOffer (true);
			CancelRegistration ();
		}

		[Test ()]
		public void TestCaseErrorMessageTooShort ()
		{
			RegisterUserWrongPassword ("kurt", "The Password must be at least 6 characters long.");
		}

		[Test ()]
		public void TestCaseErrorMessageNoDigits ()
		{
			RegisterUserWrongPassword ("Kurten!", "Passwords must have at least one digit");
		}

		[Test ()]
		public void TestCaseErrorMessageNoUppercase ()
		{
			RegisterUserWrongPassword ("kurten123!", "Passwords must have at least one uppercase");
		}

		[Test ()]
		public void TestCaseErrorMessageNoLowercase ()
		{
			RegisterUserWrongPassword ("KURTEN123!", "Passwords must have at least one lowercase");
		}

		[Test ()]
		public void TestCaseErrorMessageNoSpecialCharacter ()
		{
			RegisterUserWrongPassword ("Kurten2", "Passwords must have at least one non letter or digit character");
		}

		[Test ()]
		public void TestCaseErrorMessageWrongConfirmedPassword ()
		{
			RegisterUserWrongPassword ("Kurten123!", "Kurten12!", "The password and confirmation password do not match");
		}

		[Test ()]
		public void TestCaseErrorMessageDateTimeError ()
		{
			RegisterUserWrongDateTime ("userModel.birthdate");
		}

		[Test ()]
		public void TestCaseInvalidEventUpdate ()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
			UpdateEventSlotsTaken ();
		}
	}
}

