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
	public class Tests
	{
		private int eventID;
		Event e = new Event ();		
		public static RestClient testClient = new RestClient("http://prozum.dk:8080/api/");
		public static Random r = new Random();
		public static string testUsername, testToken = "", token_type = "";
		public static string password = "Testkurt123!";

		[TestFixtureSetUp()]
		public void StartUp()
		{
			e.Address = "Test Street 101";
			e.Date = "10-11-2015";
			e.Description = "This is a test, never mind this!";
			e.SlotsTaken = 0;
			e.SlotsTotal = 100;
			e.Title = "[Test]" + r.Next (0, 9999);
		}

		public Tests ()
		{
			testUsername = "ServerTestUser" + r.Next(1, 1000000);
		}

		[Test()]
		public void _1TestCaseRegisterUser ()
		{
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				username = testUsername,
				password = password,
				confirmPassword = password
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			Assert.IsTrue (response.StatusCode == HttpStatusCode.OK, response.Content); //Test something else
		}

		[Test()]
		public void _2TestCaseLogin()
		{
			var request = new RestRequest("token", Method.POST);

			request.AddHeader("content-type", "x-www-form-urlencoded");
			request.AddHeader ("postman-token", "a4e85886-daf2-5856-b530-12ed21af5867");
			request.AddHeader("cache-control", "no_cache");

			request.AddParameter("x-www-form-urlencoded", String.Format("username={0}&password={1}&grant_type=password", testUsername, password), ParameterType.RequestBody);

			var tokenResponse = testClient.Execute (request);

			Assert.IsFalse (tokenResponse.StatusCode == 0, "[Login failed] Likely there's no internet connection.");

			dynamic dynObj = JsonConvert.DeserializeObject (tokenResponse.Content);

			Assert.IsTrue (tokenResponse.StatusCode == HttpStatusCode.OK, tokenResponse.Content);

			testToken = dynObj.access_token;
			token_type = dynObj.token_type;

			Assert.AreNotEqual (testToken, "");
			Assert.AreNotEqual (token_type, "");
		}
			
		[Test ()]
		public void TestCaseAddReview ()
		{
			var request = buildRequest ("review", Method.POST);

			var review = new {
				rating = 3,
				reviewTect = "Koldt"//, eventID = 1234
			};
			request.AddBody(review);

			var response = testClient.Execute (request);
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);
		}
			
		[Test ()]
		public void TestCaseAddInterest ()
		{
			//Adding an interest
			var request = buildRequest ("info", Method.POST);

			var InfoUpdate = new {
				Info = 1,
				val = 5
			};
			request.AddBody (InfoUpdate);
			var response = testClient.Execute (request);
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);

		}

		[Test ()]
		public void TestCaseGetInterest ()
		{
			var request = buildRequest ("info/1", Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);
			//Testing if this interest is the same as the one added previously

			dynamic receivedInterest = JsonConvert.DeserializeObject (response.Content);

			Assert.AreEqual(5, receivedInterest, "Something went wrong " + response.Content);
		}

		[Test ()]
		public void TestCaseGetOffers ()
		{
			var request = buildRequest ("offer", Method.GET);

			var response = testClient.Execute (request);

			var offer = parseEvents (response, true);

			Assert.AreNotEqual (new Event(), offer);
		}

		[Test()]
		public void TestCaseAddEvent()
		{
			var request = buildRequest ("host", Method.POST);
			request.AddBody (e);

			var response = testClient.Execute (request);
			dynamic jVal = JsonConvert.DeserializeObject(response.Content);
			e.Id = jVal.Id;
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			dynamic dynObj = JsonConvert.DeserializeObject (response.Content);
			eventID = dynObj.Id;
		}

		[Test()]
		public void TestCaseGetEvent()
		{
			var request = buildRequest (string.Format ("host"), Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			Event receivedEvent = parseEvents (response, false);

			Assert.AreEqual (e.Id, receivedEvent.Id, "The received event was not equal to the one created");
		}
		/*[Test()]
		 * This cannot be tested as of yet
		public void TestCaseGetAttendingEvents()
		{
			var request = buildRequest (string.Format ("user/event"), Method.GET);

			var response
		}*/

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
		public void zzTestCaseDeleteEvent()
		{
			var request = buildRequest ("host/" + e.Id, Method.DELETE);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content); 

			//Now try to get event:
			request = buildRequest (string.Format ("host", e.Id), Method.GET);

			response = testClient.Execute (request);

			Event receivedEvent = parseEvents (response, false);

			Assert.AreEqual (receivedEvent, null);
		}

		private RestRequest buildRequest(string resource, Method method)
		{
			var request = new RestRequest (resource, method);

			request.AddHeader ("Authorization", "bearer " + testToken);
			request.RequestFormat = DataFormat.Json;

			return request;
		}

		private Event parseEvents(IRestResponse serverResponse, bool returnfirst){
			//Tries to convert response to events
			var events = new List<Event>();

			//Extract every single json to it's own JsonValue
			Regex reg = new Regex(@"{[^}]*}");
			var matches = reg.Matches(serverResponse.Content);

			//Generate Events from the JsonValues
			for(int i = 0; i < matches.Count; i++)
			{
				//Generate Events from the JsonValues
				try {
					dynamic jVal = JsonConvert.DeserializeObject(matches[i].Value);
					int ID = jVal.Id;
					string title = jVal.Title;
					string desc = jVal.Description;
					string dateTime = jVal.Date;
					string adress = jVal.Address;
					int slotsTotal = jVal.SlotsTotal;
					int slotsTaken = jVal.SlotsTaken;

					Event  eNew = new Event();
					eNew.Address =  adress;
					eNew.Date = dateTime;
					eNew.Description = desc;
					eNew.Id = ID;
					eNew.SlotsTaken = slotsTaken;
					eNew.SlotsTotal = slotsTotal;
					eNew.Title = title;

					events.Add(eNew);
				}
				catch
				{
				}
			}
			if (returnfirst)
				return events[0];
			else 
			{
				Event eRight = events.Find ((evnt) => evnt.Id == e.Id);
				return eRight;
			}
		}
		[Test ()]
		public void zzzTestCaseDeleteUser ()
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

