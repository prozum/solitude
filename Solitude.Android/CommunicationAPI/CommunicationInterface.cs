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

		/// <summary>
		/// Parses the error message and assigns to the latestError string.
		/// </summary>
		/// <param name="Response">Response from server.</param>
		void parseErrorMessage(IRestResponse response)
		{
			string errorContent = response.Content;
			string[] splitErrorContent = errorContent.Split(':');
			latestError = splitErrorContent [splitErrorContent.Length - 1].Trim('"', ':', '\\', '[', ']', '{', '}').Replace (".", ".\n");
		}

		bool executeAndParseResponse(IRestRequest request)
		{
			var response = client.Execute (request);

			if (response.StatusCode != HttpStatusCode.OK) {
				parseErrorMessage (response);
				return false;
			} 
			else
				return true;
		}

		#region IClientCommunication implementation
		#region OfferFetching
		/// <summary>
		/// Request the list of matches found by the server.
		/// </summary>
		/// <returns>A list of all Offers.</returns>
		public List<Offer> RequestOffers ()
		{
			var offerRequest = new RestRequest ("offer", Method.GET);

			offerRequest.AddParameter ("userToken", userToken);

			try
			{
				IRestResponse<List<Offer>> offerResponse = client.Execute<List<Offer>>(offerRequest);
				return offerResponse.Data;

			}
			catch (Exception e)
			{
				latestError = "Could not fetch the events from the server" + e.Message;
				return new List<Offer> ();
			}
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
			string serverResponse = client.Execute(eventRequest).Content;

			//Tries to convert response to events
			try {
				//Initialize variables
				var events = new List<Event>();

				//Extract every single json to it's own JsonValue
				Regex reg = new Regex(@"{[^}]*}");
				var matches = reg.Matches(serverResponse);

				//Generate Events from the JsonValues
				for(int i = 0; i < matches.Count; i++)
				{
					JsonValue jVal = System.Json.JsonObject.Parse(matches[i].Value);
					int ID = parseToInt(jVal["ID"]);
					//string title = jVal["Title"];
					string desc = jVal["Description"];
					string dateTime = jVal["Date"];
					DateTime dt = parseToDateTime(dateTime);
					string adress = jVal["Address"];

					events.Add(new Event("Missing out from server, fix it!", dt, adress, desc, 10, 10));
				}

				return events;
			} 
			catch (Exception ex)
			{
				latestError = "Could not find events\n" + ex.Message;
				return new List<Event> ();
			}
		}

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
			
		#region Notification fetching
		public void GetNotification()
		{
			throw new NotImplementedException ();
		}
		#endregion
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
				confirmedPassword = ConfirmedPassword
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
		/// <param name="id">Identifier of the user to change.</param>
		public void UpdateUser (InfoChange i)
		{
			var request = new RestRequest ("user", Method.PUT);
			request.AddBody(i);

			executeAndParseResponse (request);
		}

		/// <summary>
		/// Deletes the given user.
		/// </summary>
		/// <param name="u">User to delete.</param>
		public void DeleteUser ()
		{
			var deleteRequest = new RestRequest ("user/delete", Method.DELETE);
			deleteRequest.RequestFormat = DataFormat.Json;

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
		#endregion
		#region Event-handling
		/// <summary>
		/// Creates an event on the server.
		/// </summary>
		/// <param name="e">Event to create.</param>
		public void CreateEvent (Event e)
		{
			var request = new RestRequest ("event", Method.POST);

			request.RequestFormat = DataFormat.Json;
			request.AddObject (e);

			executeAndParseResponse (request);
		}

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
		/// Replies the offer.
		/// </summary>
		/// <param name="answer">If set to <c>true</c> the user wants to join.</param>
		/// <param name="e">Event which is being replied to.</param>
		public void ReplyOffer (bool answer, Event e)
		{
			var request = new RestRequest ("offer", Method.PUT);
			request.RequestFormat = DataFormat.Json;

			var offerReply = new {
				eventID = e.ID,
				userToken = userToken,
				reply = answer
			};
			request.AddBody(offerReply);

			executeAndParseResponse (request);
		}

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
				eventID = e.ID,
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
				reviewTect = r.ReviewText//, eventID = r.Event.id
			};
			request.AddBody(review);

			executeAndParseResponse (request);
		}
		#endregion
	}
}