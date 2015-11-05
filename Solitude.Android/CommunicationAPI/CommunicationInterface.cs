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
			var eventRequest = new RestRequest ("event", Method.GET);
			eventRequest.AddParameter ("Token", userToken);

			try 
			{
				IRestResponse<List<Event>> eventResponse = client.Execute<List<Event>>(eventRequest);
				return eventResponse.Data;
			}
			catch (Exception ex)
			{
				latestError = "Could not find events\n" + ex.Message;
				return new List<Event> ();
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
			request.AddParameter (i.Key, i.Value);

			executeAndParseResponse (request);
		}

		/// <summary>
		/// Deletes the given user.
		/// </summary>
		/// <param name="u">User to delete.</param>
		public void DeleteUser ()
		{
			var deleteRequest = new RestRequest ("user", Method.DELETE);

			deleteRequest.AddParameter ("Token", userToken);

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
			request.AddHeader("content-type", "application/x-www-form-urlencoded");
			request.AddHeader("postman-token", "a4e85886-daf2-5856-b530-12ed21af5867");
			request.AddHeader("cache-control", "no-cache");

			//Adds body including username and password and specify, that a grant_type as password is desired
			request.AddParameter("application/x-www-form-urlencoded", String.Format("username={0}&password={1}&grant_type=password", username, password), ParameterType.RequestBody);

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

			request.AddParameter ("id", e.id);

			executeAndParseResponse (request);
		}
		#endregion
		// Likely needs rewriting
		public void ReplyOffer (bool answer, Event e)
		{
			var request = new RestRequest ("offer", Method.PUT);
			request.RequestFormat = DataFormat.Json;
			if (answer)
			{
				request.AddParameter ("event_id", e.id);
				request.AddParameter ("user_token", userToken);
			}
			else
				request.AddParameter ("decline", e);
			executeAndParseResponse (request);
		}

		public void CancelReg (Event e)
		{
			var request = new RestRequest ("event", Method.DELETE);

			request.RequestFormat = DataFormat.Json;

			request.AddParameter ("event_id", e.id);
			request.AddParameter ("user_token", userToken);

			executeAndParseResponse (request);
		}
		public void PostReview(Review r)
		{
			var request = new RestRequest("review", Method.POST);
			request.RequestFormat = DataFormat.Json;

			request.AddObject (r);
			executeAndParseResponse (request);
		}
		#endregion
	}
}