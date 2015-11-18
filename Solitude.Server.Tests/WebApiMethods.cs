using System;
using Model;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Configuration;

namespace Solitude.Server.Tests
{
	public class WebApiMethods
	{		
		public Event e = new Event ();		
		public static RestClient testClient = new RestClient(ConfigurationManager.ConnectionStrings["solitude"].ConnectionString);
		public static Random r = new Random();
		public static string testUsername, testToken = "", token_type = "", testName = "Kurt Von Egelund", password = @"Testkurt+&123!";
		Offer offer = new Offer();

		public void RegisterUser ()
		{
			testUsername = "testkurt" + r.Next(1, 1000000);
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				name = testName,
				birthdate = DateTimeOffset.UtcNow,
				address = "Fiskegade",
				username = testUsername,
				password = password,
				confirmPassword = password
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			Assert.IsTrue (response.StatusCode == HttpStatusCode.OK, response.Content);
		}

		public void RegisterUserWrongPassword (string password, string correctErrorMessage)
		{
			testUsername = "testkurt" + r.Next(1, 1000000);
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				name = testName,
				birthdate = DateTimeOffset.UtcNow,
				address = "Fiskegade",
				username = testUsername,
				password = password,
				confirmPassword = password
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			string errorMessage = parseErrorMessage (response);
			Assert.IsTrue (errorMessage.Contains(correctErrorMessage), errorMessage);
		}

		public void RegisterUserWrongPassword (string password, string confirmedPassword, string correctErrorMessage)
		{
			testUsername = "testkurt" + r.Next(1, 1000000);
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				name = testName,
				birthdate = DateTimeOffset.UtcNow,
				address = "Fiskegade",
				username = testUsername,
				password = password,
				confirmPassword = confirmedPassword
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			string errorMessage = parseErrorMessage (response);
			Assert.IsTrue (errorMessage.Contains(correctErrorMessage), errorMessage);
		}

		public void RegisterUserWrongDateTime (string correctErrorMessage)
		{
			testUsername = "testkurt" + r.Next(1, 1000000);
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				name = testName,
				birthdate = "2015",
				address = "Fiskegade",
				username = testUsername,
				password = password,
				confirmPassword = password
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			string errorMessage = parseDateTimeErrorMessage (response);
			Assert.IsTrue (errorMessage.Contains(correctErrorMessage));
		}

		public void Login()
		{
			var request = new RestRequest("token", Method.POST);

			request.AddParameter("username", testUsername);
			request.AddParameter("password", password);
			request.AddParameter("grant_type", "password");

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
				rating = 5,
				reviewTect = "Han er bare så hot"//, eventID = 1234
			};
			request.AddBody(review);

			var response = testClient.Execute (request);
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);
		}

		public void AddEvent()
		{
			e.Title = "Test " + r.Next (0, 9999);

			var request = buildRequest ("host", Method.POST);

			request.AddBody (e);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			dynamic jVal = JsonConvert.DeserializeObject(response.Content);
			e.Id = jVal.Id;
		}

		public void AddCharacteristica (int Characteristica, int Value)
		{
			var request = buildRequest ("info", Method.POST);

			var InfoUpdate = new {
				Info = Characteristica,
				Value = Value
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

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(response.Content);

			var receivedEvent = events.Where ((ev) => ev.Id == e.Id).First();

			Assert.AreEqual (e.Id, receivedEvent.Id, "The received event was not equal to the one created", receivedEvent);
		}

		public void GetCharacteristica (int Characteristica, int Value)
		{
			var request = buildRequest ("info/" + Characteristica, Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The get-request failed " + response.Content);

			//Testing if the received characteristica is the same as the one added earlier
			dynamic receivedCharacteristica = JsonConvert.DeserializeObject (response.Content);

			Assert.AreEqual(Value.ToString(), receivedCharacteristica.ToString().Trim('[', '\r', '\n', ']').Trim(), "Something went wrong " + receivedCharacteristica);
		}
			
		public void GetOffers ()
		{
			var request = buildRequest ("offer", Method.GET);

			var response = testClient.Execute (request);
			//Testing if the request was executed
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			var offers = JsonConvert.DeserializeObject<IEnumerable<Offer>> (response.Content);
			offer = offers.Last();
			//Testing if the returned event has an Id.
			Assert.AreNotEqual (new Event().Id, offer.Id, "An error has occured, it is likely no offers were returned: " + response.Content);
		}

		public void ReplyOffer(bool answer)
		{
			var request = buildRequest ("offer", Method.POST);

			var Reply = new {
				Value = answer,
				EventId = offer.Id
			};
			request.AddBody (Reply);
			var response = testClient.Execute(request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);
		}

		public void UpdateEventChangeTitle()
		{
			e.Title = "Modified Test Event";
			var request = buildRequest ("host", Method.PUT);

			request.AddBody (e);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);

			//Getting the event from the server again:
			request = buildRequest (string.Format ("host"), Method.GET);

			response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The get-request was not executed correctly: " + response.Content);

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(response.Content);

			var receivedEvent = events.Where ((ev) => ev.Id == e.Id).First();

			Assert.AreEqual (e.Title, receivedEvent.Title, "The recieved event did not have the updated title.");
		} 

		public void UpdateEventSlotsTaken()
		{
			e.SlotsTaken = 1234;
			var request = buildRequest ("host", Method.PUT);

			request.AddBody (e);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content);
		} 

		public void DeleteCharacteristica (int Characteristica, int Value) 
		{
			var request = buildRequest ("info", Method.DELETE);

			var InfoToDelete = new {
				Info = Characteristica,
				Value = Value
			};
			request.AddBody (InfoToDelete);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The delete-request failed " + response.Content);

			//Testing if the characteristica is still present
			request = buildRequest ("info/" + Characteristica, Method.GET);

			response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);


			dynamic receivedCharacteristica = JsonConvert.DeserializeObject (response.Content);

			Assert.AreNotEqual(Value.ToString(), receivedCharacteristica.ToString().Trim('[', '\r', '\n', ']').Trim(), "Characteristica was not deleted correctly " + receivedCharacteristica);
		}

		public void DeleteEvent()
		{
			var request = buildRequest ("host/" + e.Id, Method.DELETE);

			request.AddBody (e.Id);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly: " + response.Content); 

			//Now try to get event:
			request = buildRequest ("host", Method.GET);

			response = testClient.Execute (request);
			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(response.Content);


			Assert.AreEqual (events.Where((ev) => ev.Id == e.Id).FirstOrDefault(), null);
		}

		public void GetAttendingEvents()
		{
			var request = buildRequest ("event", Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request was not executed correctly " + response.Content);

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(response.Content);

			Assert.AreNotEqual (new Event ().Id, events.First().Id, "Something went wrong, it is likely no events were returned: " + response.Content);
		}

		public void DeleteUser ()
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

		public void CancelRegistration ()
		{
			var request = buildRequest ("event/" + offer.Id, Method.DELETE);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The delete-request was not executed correctly: " + response.Content);

			request = buildRequest ("event", Method.GET);

			response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The get-request was not executed correctly " + response.Content);

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(response.Content);
			Event receivedEvent;
			if (events.Where ((ev) => ev.Id == e.Id).FirstOrDefault() == null)
				receivedEvent = new Event ();
			else
				receivedEvent = events.Where ((ev) => ev.Id == e.Id).First ();

			Assert.AreNotEqual (offer.Id, receivedEvent.Id, "The Registration was not cancelled correctly: " + response.Content);
		}

		public void GetUserData()
		{
			var request = buildRequest ("user", Method.GET);

			var response = testClient.Execute (request);

			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed: " + response.Content);

			dynamic receivedUserData = JsonConvert.DeserializeObject (response.Content);

			Assert.AreEqual (testUsername, receivedUserData.UserName.ToString(), "The received username is not the same as the actual: " + response.Content);
		}

		string parseErrorMessage(IRestResponse response)
		{
			string errorContent = response.Content;
			string[] splitErrorContent = errorContent.Split(':');
			return splitErrorContent [splitErrorContent.Length - 1]
				.Trim ('"', ':', '\\', '[', ']', '{', '}');
		}

		string parseDateTimeErrorMessage(IRestResponse response)
		{
			string errorContent = response.Content;
			string[] splitErrorContent = errorContent.Split(':');
			return splitErrorContent [splitErrorContent.Length - 3]
				.Trim ('"', ':', '\\', '[', ']', '{', '}');
		}

		public RestRequest buildRequest(string resource, Method method)
		{
			var request = new RestRequest (resource, method);

			request.AddHeader ("Authorization", "bearer " + testToken);
			request.RequestFormat = DataFormat.Json;

			return request;
		}
	}
}

