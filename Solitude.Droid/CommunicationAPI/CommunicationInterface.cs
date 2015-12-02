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

		/*
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
					DateTime dt = parseDate(jVal["Date"]);
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
		*/

		/*
		public DateTime parseDate(string s)
		{
			try
			{
				return DateTime.ParseExact(s, "yyyy/M/dd-hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			}
			catch
			{
				return DateTime.ParseExact(s, "yyyy/MM/dd-hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			}
		}
		*/

		/*
		/// <summary>
		/// Parses the info response.
		/// </summary>
		/// <param name="response">Response from server.</param>
		/// <param name="infoList">Info list.</param>
		/// <param name="type">Type of information to parse.</param>
		private void parseInfoResponse(IRestResponse response, List<int> infoList)
		{
			try
			{
				//Trim and split result to get single values
				string trimmed = response.Content.Trim('[', ']');
				string[] choises = trimmed.Split(',');

				//Parse each and add to information type
				foreach (var choise in choises)
					infoList.Add(int.Parse(choise));

			}
			catch
			{
				LatestError = "Could not parse interests";
			}
		}
		*/

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

			offerRequest.AddBody(new { userToken = userToken });

			var response = client.Execute(offerRequest);

			if (response.StatusCode == 0)
				LatestError = "No internet connection";
			else if (response.StatusCode != HttpStatusCode.OK)
				parseErrorMessage(response);
			else
			{
				try
				{
					return JsonConvert.DeserializeObject<List<Offer>>(response.Content);
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

			//Adds body to the request
			var body = new { userToken = userToken };
			deleteRequest.AddBody(body);

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
				LatestError = "No internet connection, please connect before logging in.";
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
			var request = buildRequest("offer", Method.POST);

			var offerReply = new { 
				EventId = e.Id, 
				Value = answer 
			};

			request.AddBody(offerReply);

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
				LatestError = "No internet connection";
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
		public bool CreateUser(string Name, string address, DateTimeOffset birthday, string Username, string Password, string ConfirmedPassword)
		{
			//Build request and user
			var request = new RestRequest("user", Method.POST);
			var user = new {
				name = Name,
				address = address,
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
				LatestError = "No connection to server";
				return new User("Sample name", "Sample address", DateTimeOffset.UtcNow);
			}
			else if (response.StatusCode == HttpStatusCode.OK)
			{
				//JsonValue jVal = System.Json.JsonObject.Parse(response.Content);

				try
				{
					return JsonConvert.DeserializeObject<User>(response.Content);

					/*
					string name = jVal["Name"];
					string adr = jVal["Address"];
					DateTime birthday = parseDate(jVal["Birthdate"]);

					return new User(name, adr, birthday);
					*/
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
					Address = e.Location,
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
				Address = e.Location,
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
				LatestError = "No internet connection";
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
				eventId = r.Event.Id };
			request.AddBody(review);

			executeAndParseResponse(request);
		}

		#endregion
	}
}