using NUnit.Framework;
using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solitude.Server.Tests
{
	[TestFixture ()]
	public class Tests : TestMethods
	{

		[TestFixtureSetUp()]
		public void StartUp()
		{
			e.Address = "Test Street 101";
			e.Date = "10-11-2015";
			e.Description = "Literally the greatest event ever";
			e.SlotsTaken = 0;
			e.SlotsTotal = 1;
			e.Title = "[Test]" + r.Next (0, 9999);
		}

		public Tests ()
		{
		}

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
		public void TestCaseAddInterest ()
		{
			RegisterUser ();
			Login ();
			AddInterest ();
		}

		[Test()]
		public void TestCaseAddEvent()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
		}
			
		[Test()]
		public void TestCaseGetEvent()
		{
			RegisterUser ();
			Login ();
			AddEvent ();
			GetEvent ();
		}

		[Test ()]
		public void TestCaseGetInterest ()
		{
			RegisterUser ();
			Login ();
			AddInterest ();
			GetInterest ();
		}

		[Test ()]
		public void TestCaseGetOffers ()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
		}

		[Test()]
		public void TestCaseReplyOffer()
		{
			RegisterUser ();
			Login ();
			GetOffers ();
			ReplyOffer ();
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
			GetAttendingEvents ();
		}

		[Test ()]
		public void TestCaseDeleteInterest ()
		{
			RegisterUser ();
			Login ();
			AddInterest ();
			DeleteInterest ();
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
	}
}

