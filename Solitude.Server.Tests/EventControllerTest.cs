using System;
using NUnit.Framework;
using RestSharp;

using Solitude.Server;
using Model;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solitude.Server.Tests
{
	[TestFixture()]
	public class EventControllerTest
	{
		private int eventID;
		Event e = new Event ();

		[TestFixtureSetUp()]
		public void StartUp()
		{
			e.Address = "Test Street 101";
			e.Date = "10-11-2015";
			e.Description = "This is a test, never mind this!";
			e.SlotsTaken = 0;
			e.SlotsTotal = 100;
			e.Title = "[Test]" + Values.r.Next (0, 9999);
		}

		[Test()]
		public void TestAddEvent()
		{
			var request = buildRequest ("event", Method.POST);
			request.AddBody (e);

			var response = Values.testClient.Execute (request);
			dynamic jVal = JsonConvert.DeserializeObject(response.Content);
			e.Id = jVal.Id;
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			dynamic dynObj = JsonConvert.DeserializeObject (response.Content);
			eventID = dynObj.Id;
		}

		[Test()]
		public void TestGetEvent()
		{
			var request = buildRequest (string.Format ("event"), Method.GET);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			Event receivedEvent = parseEvents (response);

			Assert.AreEqual (e.Id, receivedEvent.Id, "The received event was not equal to the one created");
		}

		[Test()]
		public void TestUpdateEventChangeTitle()
		{
			e.Title = "[Modified Test Event]";
			var request = buildRequest ("event", Method.PUT);

			request.AddBody (e);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			//Now get the event from the server again:
			request = buildRequest (string.Format ("event"), Method.GET);

			response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The get-request was not executed correctly: " + response.Content);

			Event receivedEvent = parseEvents (response);

			Assert.AreEqual (e.Title, receivedEvent.Title, "The recieved event did not have the updated title.");
		} 

		[Test()]
		public void zzTestDeleteEvent()
		{
			var request = buildRequest ("event/" + e.Id, Method.DELETE);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			//Now try to get event:
			request = buildRequest (string.Format ("event", e.Id), Method.GET);

			response = Values.testClient.Execute (request);

			Event receivedEvent = parseEvents (response);

			Assert.AreEqual (receivedEvent, null);
		}

		private RestRequest buildRequest(string resource, Method method)
		{
			var request = new RestRequest (resource, method);

			request.AddHeader ("Authorization", "bearer " + Values.testToken);
			request.RequestFormat = DataFormat.Json;

			return request;
		}

		private Event parseEvents(IRestResponse serverResponse){
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

			Event eRight = events.Find ((evnt) => evnt.Id == e.Id);
			return eRight;
		}
	}
}
