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
		/// <summary>
		/// Gets the lastest error.
		/// </summary>
		/// <value>The lastest error.</value>
		public string LatestError { get; private set; }

		private RestRequest buildRequest (string resource, Method method)
		{
			var req = new RestRequest(resource, method);
			req.RequestFormat = DataFormat.Json;
			req.AddHeader(HttpStrings.AUTHORIZATION, HttpStrings.BEARER + userToken);

			return req;
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
			LatestError = splitErrorContent [splitErrorContent.Length - 1]
									.Trim('"', ':', '\\', '[', ']', '{', '}')
									.Replace (".", ".\n");
		}

		/// <summary>
		/// Executes the request and parse response.
		/// </summary>
		/// <returns><c>true</c>, if and parse response was executed, <c>false</c> otherwise.</returns>
		/// <param name="request">Request.</param>
		private bool executeAndParseResponse(IRestRequest request)
		{
			var response = client.Execute (request);

			if (response.StatusCode != HttpStatusCode.OK) 
			{
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
		private int parseToInt(JsonValue value)
		{
			try 
			{
				string strVal = value.ToString();

				return int.Parse(strVal);
			}
			catch (Exception e)
			{
				LatestError = "Failed to parse event-id" + e.Message;
				return -1;
			}
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
				try 
				{
					JsonValue jVal = System.Json.JsonObject.Parse(matches[i].Value);
					int ID = parseToInt(jVal["Id"]);
					string title = jVal["Title"];
					string desc = jVal["Description"];
					DateTime dt = DateTime.Parse(jVal["Date"]);
					string adress = jVal["Address"];
					int slotsTotal = parseToInt(jVal["SlotsTotal"]);
					int slotsTaken = parseToInt(jVal["SlotsTaken"]);

					events.Add(new Event(title, dt, adress, desc, slotsTotal, slotsTaken, ID));
				}
				catch
				{
					events.Add(new Event ("title", new DateTime(1000, 01, 01), "N/A", "N/A", 0, 0));
				}
			}

			return events;
		}

		private void parseInfoResponse(IRestResponse response, List<Tuple<InfoType, int>> infoList, InfoType type)
		{
			try
			{
				string trimmed = response.Content.Trim('[', ']');
				string[] choises = trimmed.Split(',');

				foreach (var choise in choises)
				{
					infoList.Add(new Tuple<InfoType, int>(type, int.Parse(choise)));
				}
				
			}
			catch
			{
				LatestError = "Could not parse interests";
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
			var offerRequest = buildRequest ("offer", Method.GET);

			offerRequest.AddBody(new { userToken = userToken });

			var response = client.Execute(offerRequest);

			if (response.StatusCode == 0)
				LatestError = "No internet connection";
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
			var request = buildRequest ("offer", Method.PUT);

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
			var eventRequest = buildRequest ("host", Method.GET);

			//Executes request and recieves response
			var serverResponse = client.Execute(eventRequest);

			//Parses the event if status code is OK else determine the error
			if (serverResponse.StatusCode == 0)
				LatestError = "No internet connection";
			else if (serverResponse.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(serverResponse);
			else
				return parseEvents(serverResponse);

			return new List<Event>();

		}
		#endregion
		#region User-handling

		/// <summary>
		/// Creates a new user on the server.
		/// </summary>
		/// <param name="u">User to create.</param>
		public bool CreateUser (string Username, string Password, string ConfirmedPassword)
		{
			//Build request and user
			var request = new RestRequest("user/register", Method.POST);
			var user = new { username = Username, 
							 password = Password, 
							 confirmPassword = ConfirmedPassword };

			//Alters request format to json and add information
			request.RequestFormat = DataFormat.Json;
			request.AddBody(user);

			//Execute and await response
			return executeAndParseResponse(request);
		}

		/// <summary>
		/// Updates the user specified by id.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		public void AddInformation (InfoChange i)
		{
			var request = buildRequest ("info", Method.POST);

			request.AddBody(i);

			executeAndParseResponse (request);
		}

		/// <summary>
		/// Deletes the information from the server.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		public void DeleteInformation (InfoChange i)
		{
			var request = buildRequest("info", Method.DELETE);

			request.AddBody(i);

			executeAndParseResponse(request);
		}

		public List<Tuple<InfoType, int>> GetInformation ()
		{
			var foodRequest = buildRequest (string.Format("info/{0}", InfoType.FoodHabit.ToString()), Method.GET);
			var interestRequest = buildRequest(string.Format("info/{0}", InfoType.Interest), Method.GET);
			var langRequest = buildRequest(string.Format("info/{0}", InfoType.Language), Method.GET);

			var interestList = new List<Tuple<InfoType, int>>();

			var foodReponse = client.Execute(foodRequest);
			var interestResponse = client.Execute(interestRequest);
			var langResponse = client.Execute(langRequest);

			parseInfoResponse(foodReponse, interestList, InfoType.FoodHabit);
			parseInfoResponse(interestResponse, interestList, InfoType.Interest);
			parseInfoResponse(langResponse, interestList, InfoType.Language);

			return interestList;
		}

		/// <summary>
		/// Deletes the given user.
		/// </summary>
		/// <param name="u">User to delete.</param>
		public void DeleteUser ()
		{
			var deleteRequest = buildRequest ("user", Method.DELETE);

			//Adds body to the request
			var body = new { userToken = userToken };
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
				LatestError = "No internet connection, please connect before logging in.";
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
				LatestError = o ["error_description"];
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
			var request = buildRequest ("host", Method.POST);

			//And the body containing event-informatinon
			var body = new { 
				Date = e.Date.ToString(), 
				Address = e.Place,
				Title = e.Title,
				Description = e.Description,
				SlotsTaken = 0,
				SlotsTotal = e.MaxSlots
			};

			request.AddBody(body);

			var response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				JsonValue jVal = System.Json.JsonValue.Parse(response.Content);
				e.ID = parseToInt(jVal);
				return true;
			}
			else
			{
				parseErrorMessage(response);
				return false;
			}
		}

		/// <summary>
		/// Updates the event.
		/// </summary>
		/// <param name="e">Event to update</param>
		public void UpdateEvent (Event e)
		{
			var request = buildRequest ("host", Method.PUT);

			request.AddBody(new {
				Id = e.ID,
				Date = e.Date.ToString(), 
				Address = e.Place,
				Title = e.Title,
				Description = e.Description,
				SlotsTaken = e.MaxSlots - e.SlotsLeft,
				SlotsTotal = e.MaxSlots
			});

			executeAndParseResponse (request);
		}

		/// <summary>
		/// Deletes the given event.
		/// </summary>
		/// <param name="e">Event to delete.</param>
		public void DeleteEvent (Event e)
		{
			var request = buildRequest ("host/" + e.ID, Method.DELETE);

			executeAndParseResponse (request);
		}
		#endregion
		#region Offer-replies and registration cancelling
		/// <summary>
		/// Cancels the registration to the specified event.
		/// </summary>
		public void CancelReg (Event e)
		{
			var request = buildRequest ("event", Method.DELETE);

			//Add body to request
			var cancelBody = new { eventId = e.ID,
								   userToken = userToken };
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
			var request = buildRequest("review/add", Method.POST);

			//Adds a body to the request containing the reciew
			var review = new { rating = r.Rating,
							   text = r.ReviewText,
							   eventId = r.Event.ID };
			request.AddBody(review);

			executeAndParseResponse (request);
		}
		#endregion
	}
}