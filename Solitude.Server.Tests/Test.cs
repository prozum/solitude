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
			testUsername = "ServerTestUser" + r.Next(1, 1000000);
		}

		[Test()]
		public void _1TestCaseRegisterUser ()
		{
			RegisterUser ();
		}

		[Test()]
		public void _2TestCaseLogin()
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
		public void TestCasezDeleteInterest ()
		{
			var request = buildRequest ("info", Method.DELETE);

			var InfoDelete = new {
				Info = 1,
				value = 5
			};
			request.AddBody(InfoDelete);	

			var response = testClient.Execute (request);
			//Testing if the request was executed properly
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);


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
			var request = buildRequest ("offer", Method.POST);

			var reply = new {
				Value = true,
				EventID = 10
			};
			request.AddBody (reply);
			var response = testClient.Execute(request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);
		}


		[Test()]
		public void TestCasexGetAttendingEvents()
		{
			var request = buildRequest ("event", Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly " + response.Content);

			var receivedEvent = parseEvents (response, true);

			Assert.AreNotEqual (new Event ().Id, receivedEvent.Id, response.Content);
		}

		[Test()]
		public void TestCaseUpdateEventChangeTitle()
		{
			e.Title = "[Modified Test Event]";
			var request = buildRequest ("host", Method.PUT);

			request.AddBody (e);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			//Now get the event from the server again:
			request = buildRequest (string.Format ("host"), Method.GET);

			response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The get-request was not executed correctly: " + response.Content);

			Event receivedEvent = parseEvents (response, false);

			Assert.AreEqual (e.Title, receivedEvent.Title, "The recieved event did not have the updated title.");
		} 

		[Test()]
		public void TestCasezzDeleteEvent()
		{
			var request = buildRequest ("host/" + e.Id, Method.DELETE);

			request.AddBody (e.Id);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content); 

			//Now try to get event:
			request = buildRequest ("host" /*+ e.Id*/, Method.GET);

			response = testClient.Execute (request);

			Event receivedEvent = parseEvents (response, false);

			Assert.AreEqual (receivedEvent, null);
		}
			
		[Test ()]
		public void TestCasezzzDeleteUser ()
		{
			var deleteRequest = new RestRequest ("user", Method.DELETE);
			deleteRequest.RequestFormat = DataFormat.Json;

			deleteRequest.AddHeader("Authorization", "bearer " + testToken);

			//Adds body to the request
			var body = new {
				userToken = testToken
			};
			deleteRequest.AddBody(body);
			var response = testClient.Execute (deleteRequest);

			var request = new RestRequest("token", Method.POST);

			request.AddHeader("content-type", "x-www-form-urlencoded");
			request.AddHeader ("postman-token", "a4e85886-daf2-5856-b530-12ed21af5867");
			request.AddHeader("cache-control", "no_cache");

			request.AddParameter("x-www-form-urlencoded", String.Format("username={0}&password={1}&grant_type=password", testUsername, password), ParameterType.RequestBody);

			var tokenResponse = testClient.Execute (request);

			Assert.IsFalse (tokenResponse.StatusCode == HttpStatusCode.OK, "Login succeeded unexpectedly " + response.StatusCode.ToString());
		}
	}
}

