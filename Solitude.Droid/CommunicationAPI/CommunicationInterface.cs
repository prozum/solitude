using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Threading;

using Solitude.Droid;

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Text.RegularExpressions;
using Android.Content;
using Android.App;

namespace ClientCommunication
{
	public class CommunicationInterface
	{
		[Android.Runtime.Preserve(AllMembers = true)]
		private string userToken;
		private string token_type;

		RestClient client = new RestClient(HttpStrings.SERVER_URL);

		/// <summary>
		/// Gets the latest error.
		/// </summary>
		/// <value>The latest error.</value>
		public string LatestError { get; private set; }

		/// <summary>
		/// Builds a request, complete with authorization, DataFormat and method.
		/// </summary>
		/// <returns>The complete request.</returns>
		/// <param name="resource">Resource.</param>
		/// <param name="method">Method.</param>
		private RestRequest buildRequest(string resource, Method method)
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
			LatestError = splitErrorContent[splitErrorContent.Length - 1]
				.Trim('"', ':', '\\', '[', ']', '{', '}')
				.Replace(".", ".\n");
		}

		/// <summary>
		/// Executes the request and parse response.
		/// </summary>
		/// <returns><c>true</c>, if and parse response was executed, <c>false</c> otherwise.</returns>
		/// <param name="request">Request.</param>
		private bool executeAndParseResponse(IRestRequest request)
		{
			var response = client.Execute(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				parseErrorMessage(response);
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
		#endregion

		#region IClientCommunication implementation

		#region OfferFetching

		/// <summary>
		/// Request the list of matches found by the server.
		/// </summary>
		/// <returns>A list of all Offers.</returns>
		public List<Offer> RequestOffers()
		{
			var offerRequest = buildRequest("offer", Method.GET);

			var response = client.Execute(offerRequest);

			if (response.StatusCode == 0)
				LatestError = "Could not connect to server.";
			else if (response.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(response);
			else
			{
				try
				{
					var offers = JsonConvert.DeserializeObject<List<Offer>>(response.Content);
                    return offers;
				}
				catch
				{
					LatestError = "Could not parse offer";
				}
			}
				

			return new List<Offer>();
		}

		/// <summary>
		/// Deletes the given user.
		/// </summary>
		/// <param name="u">User to delete.</param>
		public void DeleteUser()
		{
			var deleteRequest = buildRequest("user", Method.DELETE);

			executeAndParseResponse(deleteRequest);
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
			request.AddParameter("username", username);
			request.AddParameter("password", password);
			request.AddParameter("grant_type", "password");
//			request.AddHeader("content-type", HttpStrings.URLENCODED);
//			request.AddHeader("postman-token", HttpStrings.CLIENT_TOKEN);
//			request.AddHeader("cache-control", HttpStrings.NO_CACHE);
			request.Timeout = 10000;

			//Adds body including username and password and specify, that a grant_type as password is desired
			//request.AddParameter(HttpStrings.URLENCODED, @"username=" + username + @"&password=" + password + @"&grant_type=password", ParameterType.RequestBody);

			//Execute and await response, parse afterwards
			var tokenResponse = client.Execute(request);

			if (tokenResponse.StatusCode == 0)
			{
				LatestError = "Could not connect to server.";
				return false;
			}

			//Tries to parse the response
			JsonValue o;
			try
			{
				o = System.Json.JsonObject.Parse(tokenResponse.Content);
			}
			catch
			{
				LatestError = "Sorry, could not login:\nUnknown server reponse";
				return false;
			}

			//Saves the token value and type and return true, if the login was successful and false otherwise
			if (tokenResponse.StatusCode == HttpStatusCode.OK)
			{
				userToken = o["access_token"];
				token_type = o["token_type"];
				return true;
			}
			else
			{
				LatestError = o["error_description"];
				return false;
			}
		}

		/// <summary>
		/// Logout from the system.
		/// </summary>
		/// <param name="activeActivity">Active activity used to start intent from.</param>
		public void Logout(Activity activeActivity)
		{
			userToken = null;

			var toLogin = new Intent(activeActivity, typeof(MainActivity));
			toLogin.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
			activeActivity.StartActivity(toLogin);
		}

		#endregion

		#region Information-handling

		/// <summary>
		/// Replies the offer.
		/// </summary>
		/// <param name="answer">If set to <c>true</c> the user wants to join.</param>
		/// <param name="e">Event which is being replied to.</param>
		public void ReplyOffer(bool answer, Event e)
		{
			RestRequest request;
			var str = string.Format("offer/{0}", e.Id);

			if (answer)
			{
				request = buildRequest(str, Method.POST);
			}
			else
			{
				request = buildRequest(str, Method.DELETE);
			}

			executeAndParseResponse(request);
		}

		#endregion

		#region Event fetching

		/// <summary>
		/// Gets the users own events.
		/// </summary>
		/// <returns>A list of the users events.</returns>
		/// <param name="n">Amount of events to find.</param>
		/// <param name="NEWEST">If set to <c>true</c> returns the newest events.</param>
		public List<Event> GetHostedEvents(int n, bool NEWEST = true)
		{
			//Builds request
			var eventRequest = buildRequest("host", Method.GET);

			//Executes request and recieves response
			var serverResponse = client.Execute(eventRequest);

			//Parses the event if status code is OK else determine the error
			if (serverResponse.StatusCode == 0)
				LatestError = "Could not connect to server.";
			else if (serverResponse.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(serverResponse);
			else
			{
				try
				{
					return JsonConvert.DeserializeObject<List<Event>>(serverResponse.Content);
				}
				catch
				{
					LatestError = "Could not parse event";
				}
			} 

			return new List<Event>();
		}

		#endregion

		#region User-handling

		/// <summary>
		/// Creates a new user on the server.
		/// </summary>
		/// <param name="u">User to create.</param>
		public bool CreateUser(string Name, string location, DateTimeOffset birthday, string Username, string Password, string ConfirmedPassword)
		{
			//Build request and user
			var request = new RestRequest("user", Method.POST);
			var user = new {
				name = Name,
				location = location,
				birthdate = birthday, //string.Format("{0}/{1}/{2}-12:00:00", birthday.Year, birthday.Month, birthday.Day), 
				UserName = Username, 
				Password = Password, 
				confirmPassword = ConfirmedPassword };

			//Alters request format to json and add information
			request.RequestFormat = DataFormat.Json;
			request.AddBody(user);

			//Execute and await response
			return executeAndParseResponse(request);
		}

		public User GetUserData()
		{
			var request = buildRequest("user", Method.GET);

			var response = client.Execute(request);

			if (response.StatusCode == 0)
			{
				LatestError = "Could not connect to server.";
				return new User("Sample name", "Sample address", DateTimeOffset.UtcNow);
			}
			else if (response.StatusCode == HttpStatusCode.OK)
			{
				//JsonValue jVal = System.Json.JsonObject.Parse(response.Content);

				try
				{
					return JsonConvert.DeserializeObject<User>(response.Content);
				}
				catch
				{
					LatestError = "Could not find user data";
				}

				return new User("Sample name", "Sample address", DateTimeOffset.UtcNow);
			}
			else
			{
				parseErrorMessage(response);
				return new User("Sample name", "Sample address", DateTimeOffset.UtcNow);
			}
		}

		#endregion

		#region Information-handling

		/// <summary>
		/// Updates the user specified by id.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		public void AddInformation(InfoChange i)
		{
			var request = buildRequest("info", Method.POST);

			request.AddBody(i);

			executeAndParseResponse(request);
		}

		/// <summary>
		/// Deletes the information from the server.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		public void DeleteInformation(InfoChange i)
		{
			var request = buildRequest("info", Method.DELETE);

			request.AddBody(i);

			executeAndParseResponse(request);
		}

		/// <summary>
		/// Gets the information of current user.
		/// </summary>
		/// <returns>The information.</returns>
		public List<int>[] GetInformation()
		{
			var foodRequest = buildRequest(string.Format("info/{0}", (int) InfoType.FoodHabit), Method.GET);
			var interestRequest = buildRequest(string.Format("info/{0}", (int) InfoType.Interest), Method.GET);
			var langRequest = buildRequest(string.Format("info/{0}", (int) InfoType.Language), Method.GET);

			var interestList = new List<int>[Enum.GetValues(typeof(InfoType)).Length];

			var foodReponse = client.Execute(foodRequest);
			var interestResponse = client.Execute(interestRequest);
			var langResponse = client.Execute(langRequest);

			try
			{
				interestList[(int)InfoType.FoodHabit] = JsonConvert.DeserializeObject<List<int>>(foodReponse.Content);
				interestList[(int)InfoType.Interest] = JsonConvert.DeserializeObject<List<int>>(interestResponse.Content);
				interestList[(int)InfoType.Language] = JsonConvert.DeserializeObject<List<int>>(langResponse.Content);
			}
			catch
			{
				LatestError = "Could not parse info";
			}

			return interestList;
		}

		#endregion

		#region Event-handling

		/// <summary>
		/// Creates an event on the server.
		/// </summary>
		/// <param name="e">Event to create.</param>
		public bool CreateEvent(Event e)
		{
			var request = buildRequest("host", Method.POST);

			//And the body containing event-informatinon
			var body = new 
				{ 
					Date = e.Date,
					Location = e.Location,
					Title = e.Title,
					Description = e.Description,
					SlotsTaken = 0,
					SlotsTotal = e.SlotsTotal
				};

			request.AddBody(body);

			var response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				JsonValue jVal = System.Json.JsonValue.Parse(response.Content);
				e.Id = jVal["Id"];
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
		public bool UpdateEvent(Event e)
		{
			var request = buildRequest("host", Method.PUT);

			request.AddBody(new {
				Id = e.Id,
				Date = e.Date,
				Location = e.Location,
				Title = e.Title,
				Description = e.Description,
				SlotsTaken = e.SlotsTotal - e.SlotsTaken,
				SlotsTotal = e.SlotsTotal
			});

			return executeAndParseResponse(request);
		}

		/// <summary>
		/// Deletes the given event.
		/// </summary>
		/// <param name="e">Event to delete.</param>
		public void DeleteEvent(Event e)
		{
			var request = buildRequest("host/" + e.Id, Method.DELETE);

			executeAndParseResponse(request);
		}

		/// <summary>
		/// Gets the users own events.
		/// </summary>
		/// <returns>A list of the users events.</returns>
		/// <param name="n">Amount of events to find.</param>
		/// <param name="NEWEST">If set to <c>true</c> returns the newest events.</param>
		public List<Event> GetJoinedEvents(int n, bool NEWEST = true)
		{
			//Builds request
			var eventRequest = buildRequest("event", Method.GET);

			//Executes request and recieves response
			var serverResponse = client.Execute(eventRequest);

			//Parses the event if status code is OK else determine the error
			if (serverResponse.StatusCode == 0)
				LatestError = "Could not connect to server.";
			else if (serverResponse.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(serverResponse);
			else
			{
				try
				{
					return JsonConvert.DeserializeObject<List<Event>>(serverResponse.Content);
				}
				catch
				{
					LatestError = "Could not parse event";
				}
			}

			return new List<Event>();
		}

		#endregion

		#region Offer-replies and registration cancelling

		/// <summary>
		/// Cancels the registration to the specified event.
		/// </summary>
		public void CancelReg(Event e)
		{
			var request = buildRequest(string.Format("event/{0}", e.Id), Method.DELETE);

			//Add body to request
			var cancelBody = new { eventId = e.Id,
				userToken = userToken };
			request.AddBody(cancelBody);

			executeAndParseResponse(request);
		}

		#endregion
		#endregion
	}
}