using System;
using NUnit.Framework;
using RestSharp;

using Solitude.Server;
using Model;
using System.Net;
using Newtonsoft.Json;

namespace Solitude.Server.Tests
{
	[TestFixture()]
	public class EventControllerTest
	{
		private int eventID;
		Event e = new Event ();

		[SetUp()]
		public void StartUp()
		{
			e.Address = "Test Street 101";
			e.Date = "10-11-2015";
			e.Description = "This is a test, never mind this!";
			e.SlotsTaken = 0;
			e.SlotsTotal = 100;
			e.Title = "[Test]" + Values.r.Next (0, 9999);
			e.UserId = "0";
		}

		[Test()]
		public void TestAddEvent()
		{
			var request = buildRequest ("event/add", Method.POST);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			dynamic dynObj = JsonConvert.DeserializeObject (response.Content);
			eventID = dynObj.Id;
		}

		[Test()]
		public void TestGetEvent()
		{
			var request = buildRequest (string.Format ("event/{0}", e.Id), Method.GET);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			Event receivedEvent = JsonConvert.DeserializeObject<Event> (response.Content);

			Assert.AreEqual (e, receivedEvent, "The recieved event was not equal to the one created");
		}

		[Test()]
		public void TestUpdateEventChangeTitle()
		{
			e.Title = "[Modified Test Event]";
			var request = buildRequest ("event/update", Method.PUT);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed corrextly: " + response.Content);

			//Now get the event from the server again:
			request = buildRequest (string.Format ("event/{0}", e.Id), Method.GET);

			response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The get-request was not executed correctly: " + response.Content);

			Event receivedEvent = JsonConvert.DeserializeObject<Event> (response.Content);

			Assert.AreEqual (e.Title, receivedEvent.Title, "The recieved event did not have the updated title.");
		}

		[Test()]
		public void zzTestDeleteEvent()
		{
			var request = buildRequest ("event/delete", Method.DELETE);

			var response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			//Now try to get event:
			request = buildRequest (string.Format ("event/{0}", e.Id), Method.GET);

			response = Values.testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.NotFound, response.StatusCode, "The event was found on server and thus not deleted.");
		}

		private RestRequest buildRequest(string resource, Method method)
		{
			var request = new RestRequest (resource, method);

			request.AddHeader ("Authorization", "bearer " + Values.testToken);
			request.RequestFormat = DataFormat.Json;

			return request;
		}
	}
}
