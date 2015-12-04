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
	public class WebApiMethods : RestClient
	{
		public RestRequest Request;
		public IRestResponse Response;
		public Offer Offer = new Offer();
		public Event Event = new Event ()
		{
			Location = "Best Street 666",
			Date = DateTimeOffset.UtcNow.AddDays(1),
			Description = "Literally the greatest event ever",
			SlotsTaken = 0,
			SlotsTotal = 50,
			Title = "Best"
		};
		public User User = new User()
		{
			Name = "Sir John Fisher",
			Location = "1st Fishstreet",
			Birthdate = DateTimeOffset.UtcNow,
			Password = "passw1",
			ConfirmPassword = "passw1"
		};
		public string Token;

		public WebApiMethods() : base(ConfigurationManager.ConnectionStrings["solitude"].ConnectionString)
		{
		}

		public void ExecuteRequest(HttpStatusCode expected = HttpStatusCode.OK)
		{
			Response = Execute(Request);
			Assert.AreEqual(expected, Response.StatusCode, "The request failed: " + Response.Content);
		}

		public void BuildRequest(string resource, Method method, object body = null)
		{
			Request = new RestRequest (resource, method);

			Request.AddHeader ("Authorization", "bearer " + Token);
			Request.RequestFormat = DataFormat.Json;
			if (body != null)
				Request.AddBody (body);
		}

		public void RegisterUser ()
		{
			User.Username = "user-" + Guid.NewGuid();
			BuildRequest("user", Method.POST, User);
			ExecuteRequest();
		}

		public void Login()
		{
			Request = new RestRequest("token", Method.POST);
			Request.AddParameter("username", User.Username);
			Request.AddParameter("password", User.Password);
			Request.AddParameter("grant_type", "password");
			ExecuteRequest();

			dynamic dynObj = JsonConvert.DeserializeObject (Response.Content);
			Assert.AreEqual("bearer", dynObj.token_type.ToString());
			Assert.AreNotEqual ("", dynObj.access_token.ToString());

			Token = dynObj.access_token;
		}

		public void RegisterUserWrongPassword (string password, string correctErrorMessage)
		{
			RegisterUserWrongPassword(password, password, correctErrorMessage);
		}

		public void RegisterUserWrongPassword (string password, string confirmPassword, string correctErrorMessage)
		{
			var user = new 
			{
				name = User.Name,
				birthdate = User.Birthdate,
				location = User.Location,
				username = "user-" + Guid.NewGuid(),
				password = password,
				confirmPassword = confirmPassword
			};
			BuildRequest("user", Method.POST, user);
			ExecuteRequest(HttpStatusCode.BadRequest);

			string errorMessage = parseErrorMessage (Response);
			Assert.IsTrue (errorMessage.Contains(correctErrorMessage), errorMessage);
		}

		public void RegisterUserWrongDateTime (string correctErrorMessage)
		{
			var user = new 
			{
				name = User.Name,
				birthdate = "2015",
				address = User.Location,
				username = "user-" + Guid.NewGuid(),
				password = User.Password,
				confirmPassword = User.ConfirmPassword
			};
			BuildRequest("user", Method.POST, user);
			ExecuteRequest(HttpStatusCode.BadRequest);

			Assert.IsTrue (Response.Content.Contains(correctErrorMessage), Response.Content);
		}

		public void AddReview ()
		{
			var review = new 
			{
				rating = 5,
				reviewTect = "Han er bare så hot"
			};
			BuildRequest("review", Method.POST, review);
			ExecuteRequest();
		}

		public void AddEvent()
		{
			Event.Title = "event-" + Guid.NewGuid();
			BuildRequest("host", Method.POST, Event);
			ExecuteRequest();

            Event.Id = JsonConvert.DeserializeObject<Event>(Response.Content).Id;
		}

		public void AddCharacteristica (int Characteristica, int Value)
		{
			var info = new
			{
				Info = Characteristica,
				Value = Value
			};
			BuildRequest("info", Method.POST, info);
			ExecuteRequest();
		}

		public void GetEvent()
		{
			BuildRequest ("host", Method.GET);
			ExecuteRequest();

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(Response.Content);
			var receivedEvent = events.Where ((ev) => ev.Id == Event.Id).First();
			Assert.AreEqual (Event.Id, receivedEvent.Id, "The received event was not equal to the one created", receivedEvent);
		}

		public void GetCharacteristica (int Characteristica, int Value)
		{
			BuildRequest ("info/" + Characteristica, Method.GET);
			ExecuteRequest();

			//Testing if the received characteristica is the same as the one added earlier
			dynamic receivedCharacteristica = JsonConvert.DeserializeObject (Response.Content);
			Assert.AreEqual(Value.ToString(), receivedCharacteristica.ToString().Trim('[', '\r', '\n', ']').Trim(), "Something went wrong " + receivedCharacteristica);
		}
			
		public void GetOffers ()
		{
			BuildRequest("offer", Method.GET);
			ExecuteRequest();

			//Testing if the returned event has an Id.
			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(Response.Content);
			Offer.Id = events.Last().Id;
			Assert.AreNotEqual("", Offer.Id, "An error has occured, it is likely no offers were returned: " + Response.Content);
		}

		public void AcceptOffer()
		{
			BuildRequest ("offer/" + Offer.Id, Method.POST);
			ExecuteRequest();
		}

		public void DeclineOffer()
		{
			BuildRequest ("offer/" + Offer.Id, Method.DELETE);
			ExecuteRequest();
		}

		public void UpdateEventChangeTitle()
		{
			Event.Title = "Modified Test Event";
			BuildRequest ("host", Method.PUT, Event);
			ExecuteRequest();

			//Getting the event from the server again:
			BuildRequest ("host", Method.GET);
			ExecuteRequest();

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(Response.Content);
			var receivedEvent = events.Where((ev) => ev.Id == Event.Id).First();
			Assert.AreEqual(Event.Title, receivedEvent.Title, "The recieved event did not have the updated title.");
		} 

		public void UpdateEventSlotsTaken()
		{
			Event.SlotsTaken = 1234;
			BuildRequest ("host", Method.PUT, Event);
			ExecuteRequest();
		} 

		public void DeleteCharacteristica (int Characteristica, int Value) 
		{
			var info = new 
			{
				Info = Characteristica,
				Value = Value
			};
			BuildRequest ("info", Method.DELETE, info);
			ExecuteRequest();

			//Testing if the characteristica is still present
			BuildRequest ("info/" + Characteristica, Method.GET);
			ExecuteRequest();

			dynamic receivedCharacteristica = JsonConvert.DeserializeObject (Response.Content);
			Assert.AreNotEqual(Value.ToString(), receivedCharacteristica.ToString().Trim('[', '\r', '\n', ']').Trim(), "Characteristica was not deleted correctly " + receivedCharacteristica);
		}

		public void DeleteEvent()
		{
			BuildRequest ("host/" + Event.Id, Method.DELETE);
			ExecuteRequest();

			//Now try to get event:
			BuildRequest("host", Method.GET);
			ExecuteRequest();

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(Response.Content);
			Assert.IsNull(events.Where((ev) => ev.Id == Event.Id).FirstOrDefault());
		}

		public void GetAttendingEvents()
		{
			BuildRequest("event", Method.GET);
			ExecuteRequest();

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(Response.Content);
			Assert.AreNotEqual("", events.First().Id, "Something went wrong, it is likely no events were returned: " + Response.Content);
		}

		public void DeleteUser ()
		{
			BuildRequest("user", Method.DELETE);
			ExecuteRequest();

			Request = new RestRequest("token", Method.POST);
			Request.AddParameter("username", User.Username);
			Request.AddParameter("password", User.Password);
			Request.AddParameter("grant_type", "password");
			ExecuteRequest(HttpStatusCode.BadRequest);
		}

		public void CancelRegistration ()
		{
			BuildRequest("event/" + Offer.Id, Method.DELETE);
			ExecuteRequest();

			BuildRequest ("event", Method.GET);
			ExecuteRequest();

			var events = JsonConvert.DeserializeObject<IEnumerable<Event>>(Response.Content);
			Assert.IsFalse(events.Where ((ev) => ev.Id == Event.Id).Any<Event>(), "The Registration was not cancelled correctly: " + Response.Content);
		}

		public void GetUserData()
		{
			BuildRequest ("user", Method.GET);
			ExecuteRequest();

			dynamic receivedUserData = JsonConvert.DeserializeObject (Response.Content);
			Assert.AreEqual (User.Username, receivedUserData.UserName.ToString(), "The received username is not the same as the actual: " + Response.Content);
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
	}
}

