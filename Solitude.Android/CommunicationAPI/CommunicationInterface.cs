using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Threading;

using DineWithaDane.Android;


using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.RegularExpressions;
using Android.Content;
using Android.App;

namespace ClientCommunication
{
	public class CommunicationInterface //: IClientCommunication
	{
		private string userToken;
		private string token_type;

		RestClient client = new RestClient (HttpStrings.SERVER_URL);
		private string latestError;
		/// <summary>
		/// Gets the lastest error.
		/// </summary>
		/// <value>The lastest error.</value>
		public string LatestError {
			get { return latestError; }
		}

		#region parses
		/// <summary>
		/// Parses the error message and assigns to the latestError string.
		/// </summary>
		/// <param name="Response">Response from server.</param>
		private void parseErrorMessage(IRestResponse response)
		{
			string errorContent = response.Content;
			string[] splitErrorContent = errorContent.Split(':');
			latestError = splitErrorContent [splitErrorContent.Length - 1].Trim('"', ':', '\\', '[', ']', '{', '}').Replace (".", ".\n");
		}

		/// <summary>
		/// Executes the request and parse response.
		/// </summary>
		/// <returns><c>true</c>, if and parse response was executed, <c>false</c> otherwise.</returns>
		/// <param name="request">Request.</param>
		private bool executeAndParseResponse(IRestRequest request)
		{
			var response = client.Execute (request);

			if (response.StatusCode != HttpStatusCode.OK) {
				parseErrorMessage (response);
				return false;
			} 
			else
				return true;
		}

		/// <summary>
		/// Parses a JsonValue to int.
		/// </summary>
		/// <returns>An int with same value as the JsonValue.</returns>
		/// <param name="value">Value.</param>
		private int parseToInt(JsonValue value){
			try 
			{
				string strVal = value.ToString();

				return int.Parse(strVal);
			}
			catch (Exception e)
			{
				latestError = "Failed to parse event-id" + e.Message;
				return -1;
			}
		}

		/// <summary>
		/// Parses a string to DateTime object.
		/// </summary>
		/// <returns>The DateTime specified in the string.</returns>
		/// <param name="dt">DateTime as a string.</param>
		private DateTime parseToDateTime(string dt){
			string[] values = dt.Split('-');

			try {
				int date = int.Parse(values[0]);
				int month = int.Parse(values[1]);
				int year = int.Parse(values[2]);

				return new DateTime(year, month, date);
			}
			catch (Exception e)
			{
				latestError = "Couldn't convert date " + e.Message;
				return DateTime.Today;
			}
		}
		#endregion

		#region IClientCommunication implementation
		#region OfferFetching
		/// <summary>
		/// Request the list of matches found by the server.
		/// </summary>
		/// <returns>A list of all Offers.</returns>
		public List<Event> RequestOffers ()
		{
			var offerRequest = new RestRequest ("offer", Method.GET);
			offerRequest.RequestFormat = DataFormat.Json;

			offerRequest.AddBody(new {
				userToken = userToken
			});

			var response = client.Execute(offerRequest);

			if (response.StatusCode == 0)
				latestError = "No internet connection";
			else if (response.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(response);
			else
				return parseEvents(response);

			return new List<Event>();
		}

		/// <summary>
		/// Replies the offer.
		/// </summary>
		/// <param name="answer">If set to <c>true</c> the user wants to join.</param>
		/// <param name="e">Event which is being replied to.</param>
		public void ReplyOffer (bool answer, Event e)
		{
			var request = new RestRequest ("offer", Method.PUT);
			request.RequestFormat = DataFormat.Json;

			var offerReply = new {
				eventId = e.ID,
				userToken = userToken,
				reply = answer
			};
			request.AddBody(offerReply);

			executeAndParseResponse (request);
		}
		#endregion
		#region Event fetching
		/// <summary>
		/// Gets the users own events.
		/// </summary>
		/// <returns>A list of the users events.</returns>
		/// <param name="n">Amount of events to find.</param>
		/// <param name="NEWEST">If set to <c>true</c> returns the newest events.</param>
		public List<Event> GetOwnEvents (int n, bool NEWEST = true)
		{
			//Builds request
			var eventRequest = new RestRequest ("event", Method.GET);

			//Executes request and recieves response
			var serverResponse = client.Execute(eventRequest);

			//Parses the event if status code is OK else determine the error
			if (serverResponse.StatusCode == 0)
				latestError = "No internet connection";
			else if (serverResponse.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(serverResponse);
			else
				return parseEvents(serverResponse);

			return new List<Event>();

		}

		/// <summary>
		/// Parses the events from the server response.
		/// </summary>
		/// <returns>The events.</returns>
		/// <param name="serverResponse">Server response.</param>
		private List<Event> parseEvents(IRestResponse serverResponse){
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
					JsonValue jVal = System.Json.JsonObject.Parse(matches[i].Value);
					int ID = parseToInt(jVal["Id"]);
					string title = jVal["Title"];
					string desc = jVal["Description"];
					string dateTime = jVal["Date"];
					DateTime dt = parseToDateTime(dateTime);
					string adress = jVal["Address"];
					int slotsTotal = parseToInt(jVal["SlotsTotal"]);
					int slotsTaken = parseToInt(jVal["SlotsTaken"]);

					events.Add(new Event(title, dt, adress, desc, slotsTaken, slotsTotal - slotsTaken));
				}
				catch
				{
					//string title = "jVal["Title"] + " [Some information was missing, sorry!]"

					events.Add(new Event ("title", new DateTime(0000, 00, 00), "N/A", "N/A", 0, 0));
				}
			}

			return events;
		}
			
//		#region Notification fetching
//		public void GetNotification()
//		{
//			throw new NotImplementedException ();
//		}
//		#endregion
		#endregion
		#region User-handling

		/// <summary>
		/// Creates a new user on the server.
		/// </summary>
		/// <param name="u">User to create.</param>
		public bool CreateUser (string Username, string Password, string ConfirmedPassword)
		{
			//Build request and user
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				username = Username,
				password = Password,
				confirmPassword = ConfirmedPassword
			};

			//Alters request format to json and add information
			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);

			//Execute and await response
			return executeAndParseResponse (request);
		}


		/// <summary>
		/// Updates the user specified by id.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		public void AddInformation (InfoChange i)
		{
			var request = new RestRequest ("info/add", Method.POST);

			request.AddHeader(HttpStrings.AUTHORIZATION, HttpStrings.BEARER + userToken);

			request.AddBody(i);

			executeAndParseResponse (request);
		}

		/// <summary>
		/// Deletes the information from the server.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		public void DeleteInformation (InfoChange i)
		{
			var request = new RestRequest("info/delete", Method.DELETE);

			request.AddHeader(HttpStrings.AUTHORIZATION, HttpStrings.BEARER + userToken);

			request.AddBody(i);

			executeAndParseResponse(request);
		}

		/// <summary>
		/// Deletes the given user.
		/// </summary>
		/// <param name="u">User to delete.</param>
		public void DeleteUser ()
		{
			var deleteRequest = new RestRequest ("user/delete", Method.DELETE);
			deleteRequest.RequestFormat = DataFormat.Json;

			deleteRequest.AddHeader(HttpStrings.AUTHORIZATION, HttpStrings.BEARER + userToken);

			//Adds body to the request
			var body = new {
				userToken = userToken
			};
			deleteRequest.AddBody(body);

			executeAndParseResponse (deleteRequest);
		}

		/// <summary>
		/// Login with specified username and password.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public bool Login(string username, string password)
		{
			//Generates the token-request
			var request = new RestRequest("token", Method.POST);

			//Adds headers to notify server that it's a token-request
			request.AddHeader("content-type", HttpStrings.URLENCODED);
			request.AddHeader("postman-token", HttpStrings.CLIENT_TOKEN);
			request.AddHeader("cache-control", HttpStrings.NO_CACHE);

			//Adds body including username and password and specify, that a grant_type as password is desired
			request.AddParameter(HttpStrings.URLENCODED, String.Format("username={0}&password={1}&grant_type=password", username, password), ParameterType.RequestBody);

			//Execute and await response, parse afterwards
			var tokenResponse = client.Execute (request);

			if (tokenResponse.StatusCode == 0)
			{
				latestError = "No internet connection, please connect before logging in.";
				return false;
			}

			JsonValue o = System.Json.JsonObject.Parse(tokenResponse.Content);

			//Saves the user and return true, if the login was successful and false otherwise
			if (tokenResponse.StatusCode == HttpStatusCode.OK)
			{
				userToken = o ["access_token"];
				token_type = o ["token_type"];
				return true;
			}
			else
			{
				latestError = o ["error_description"];
				return false;
			}
		}

		/// <summary>
		/// Logout from the system.
		/// </summary>
		/// <param name="activeActivity">Active activity used to start intent from.</param>
		public void Logout(Activity activeActivity){
			userToken = null;

			var toLogin = new Intent(activeActivity, typeof(MainActivity));
			toLogin.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
			activeActivity.StartActivity(toLogin);
		}

		#endregion
		#region Event-handling
		/// <summary>
		/// Creates an event on the server.
		/// </summary>
		/// <param name="e">Event to create.</param>
		public bool CreateEvent (Event e)
		{
			var request = new RestRequest ("event/add", Method.POST);

			request.RequestFormat = DataFormat.Json;

			//Add authorization header:
			request.AddHeader(HttpStrings.AUTHORIZATION, HttpStrings.BEARER + userToken);

			//And the body containing event-informatino
			var body = new {
				Date = e.Date.ToString(),
				Adress = e.Place,
				Title = e.Title,
				Description = e.Description,
				SlotsTaken = 0,
				SlotsTotal = e.MaxSlots
			};

			request.AddBody(body);

			var response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				JsonValue jVal = new JsonValue(response.Content);
				e.ID = parseToInt(jVal);
				return true;
			}
			else
			{
				parseErrorMessage(response.Content);
			}
		}

		/// <summary>
		/// Updates the event.
		/// </summary>
		/// <param name="e">Event to update</param>
		public void UpdateEvent (Event e)
		{
			var request = new RestRequest ("event", Method.PUT);
			request.RequestFormat = DataFormat.Json;
			request.AddBody (e);

			executeAndParseResponse (request);
		}

		/// <summary>
		/// Deletes the given event.
		/// </summary>
		/// <param name="e">Event to delete.</param>
		public void DeleteEvent (Event e)
		{
			var request = new RestRequest ("event", Method.DELETE);

			request.AddParameter ("id", e);

			executeAndParseResponse (request);
		}
		#endregion
		#region Offer-replies and registration cancelling
		/// <summary>
		/// Cancels the registration to the specified event.
		/// </summary>
		public void CancelReg (Event e)
		{
			//Generate request and set DataFormat
			var request = new RestRequest ("event", Method.DELETE);
			request.RequestFormat = DataFormat.Json;

			//Add body to request
			var cancelBody = new {
				eventId = e.ID,
				userToken = userToken
			};
			request.AddBody(cancelBody);

			executeAndParseResponse (request);
		}
		#endregion

		/// <summary>
		/// Posts the review to the server.
		/// </summary>
		/// <param name="r">The review to post.</param>
		public void PostReview(Review r)
		{
			var request = new RestRequest("review/add", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader(HttpStrings.AUTHORIZATION, HttpStrings.BEARER + userToken);

			//Adds a body to the request containing the reciew
			var review = new {
				rating = r.Rating,
				text = r.ReviewText,
				eventId = r.Event.ID
			};
			request.AddBody(review);

			executeAndParseResponse (request);
		}
		#endregion
	}
}