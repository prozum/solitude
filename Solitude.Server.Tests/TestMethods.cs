using System;
using Model;
using System.Collections.Generic;
using RestSharp;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;

namespace Solitude.Server.Tests
{
	public class TestMethods
	{		
		public int eventID;
		public Event e = new Event ();		
		public static RestClient testClient = new RestClient("http://prozum.dk:8080/api/");
		public static Random r = new Random();
		public static string testUsername, testToken = "", token_type = "";
		public static string password = "Testkurt123!";
		public TestMethods ()
		{

		}
		public void RegisterUser ()
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

		public void Login()
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

		public void AddReview ()
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

		public void AddEvent()
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

		public void AddInterest ()
		{
			//Adding an interest
			var request = buildRequest ("info", Method.POST);

			var InfoUpdate = new {
				Info = 1,
				Value = 5
			};
			request.AddBody (InfoUpdate);
			var response = testClient.Execute (request);
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);
		}

		public void GetEvent()
		{
			var request = buildRequest (string.Format ("host"), Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			Event receivedEvent = parseEvents (response, false);

			Assert.AreEqual (e.Id, receivedEvent.Id, "The received event was not equal to the one created");
		}

		public void GetInterest ()
		{
			var request = buildRequest ("info/1", Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);

			//Testing if this interest is the same as the one added previously

			dynamic receivedInterest = JsonConvert.DeserializeObject (response.Content);

			Assert.AreEqual(5, receivedInterest, "Something went wrong " + response.Content);
		}

		public void GetOffers ()
		{
			var request = buildRequest ("offer", Method.GET);

			var response = testClient.Execute (request);
			//Testing if the request was executed
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			var offer = parseEvents (response, true);
			//Testing if the returned event has an Id. Will later test if it is the correct Id
			Assert.AreNotEqual (new Event().Id, offer.Id, "An error has occured, it is likely no offers were returned");
		}

		public void ReplyOffer()
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

		public RestRequest buildRequest(string resource, Method method)
		{
			var request = new RestRequest (resource, method);

			request.AddHeader ("Authorization", "bearer " + testToken);
			request.RequestFormat = DataFormat.Json;

			return request;
		}

		public Event parseEvents(IRestResponse serverResponse, bool returnfirst){
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
			if (returnfirst) {
				if (events.Count == 0)
					return new Event();
				return events[0];
			}
			else 
			{
				Event eRight = events.Find ((evnt) => evnt.Id == e.Id);
				return eRight;
			}
		}
	}
}

