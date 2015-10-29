using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;

using DineWithaDane.Android;

using Newtonsoft.Json;

namespace ClientCommunication
{
	public class CommunicationInterface : IClientCommunication
	{
		private int userID;

		public CommunicationInterface (int userID)
		{
			this.userID = userID;
		}
		public CommunicationInterface () { }

		/// <summary>
		/// Builds a get URL for the specified resource and id.
		/// </summary>
		/// <returns>The get URL.</returns>
		/// <param name="resource">Resource.</param>
		/// <param name="id">Identifier.</param>
		private HttpWebRequest buildGetRequest(string resource, int id)
		{
			string url = HttpStrings.SERVER_URL + string.Format("{0}/{1}", resource, id);
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create (new Uri(url));
			request.ContentType = HttpStrings.JSON_REQUEST;
			request.Method = HttpStrings.GET;

			return request;
		}

		/// <summary>
		/// Builds a post request of specified resource.
		/// </summary>
		/// <returns>The post request.</returns>
		/// <param name="resource">Resource.</param>
		private HttpWebRequest buildPostRequest(string resource)
		{
			string url = HttpStrings.SERVER_URL + resource;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = HttpStrings.JSON_REQUEST;
			request.Method = HttpStrings.POST;

			return request;
		}

		/// <summary>
		/// Builds a delete request on specified resource and id.
		/// </summary>
		/// <returns>The delete request.</returns>
		/// <param name="resource">Resource.</param>
		/// <param name="id">Identifier.</param>
		private HttpWebRequest buildDeleteRequest(string resource, int id)
		{
			string url = HttpStrings.SERVER_URL + string.Format("{0}/{1}", resource, id);
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create (new Uri(url));
			request.Method = HttpStrings.DELETE;

			return request;
		}

		/// <summary>
		/// Builds a put request on specified resource and id.
		/// </summary>
		/// <returns>The put request.</returns>
		/// <param name="resource">Resource.</param>
		/// <param name="id">Identifier.</param>
		private HttpWebRequest buildPutRequest(string resource, int id)
		{
			string url = HttpStrings.SERVER_URL + string.Format("{0}/{1}", resource, id);
			HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create (new Uri(url));
			request.ContentType = HttpStrings.JSON_REQUEST;
			request.Method = HttpStrings.PUT;

			return request;
		}

		/// <summary>
		/// Converts a String to a byte array.
		/// </summary>
		/// <returns>A byte array containing bytes of the string.</returns>
		/// <param name="str">String to convert.</param>
		private byte[] stringToByteArray(string str)
		{
			//Convert the eventJson-string to a byte array
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy (str.ToCharArray (), 0, bytes, 0, bytes.Length);

			return bytes;
		}

		#region IClientCommunication implementation
		#region OfferFetching
		/// <summary>
		/// Request the list of matches found by the server.
		/// </summary>
		/// <returns>A list of all Offers.</returns>
		public async Task<List<Offer>> RequestOffers ()
		{
			//JsonValue response = await fetchOffers ();
			//List<Offer> allOffers = parseOffers (response);

			//foreach (var offer in allOffers) {
			//	Console.WriteLine (offer);
			//}

			//return allOffers;
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fetchs the events from the server.
		/// </summary>
		/// <returns>A Json-string containing the events.</returns>
//		private async Task<JsonValue> fetchOffers()
//		{
//			
//		}
			
//		private List<Offer> parseOffers(JsonValue serverResponse)
//		{
//			
//		}
		#endregion
		#region Event fethcing
		/// <summary>
		/// Gets the users own events.
		/// </summary>
		/// <returns>A list of the users events.</returns>
		/// <param name="n">Amount of events to find.</param>
		/// <param name="NEWEST">If set to <c>true</c> returns the newest events.</param>
		public async Task<List<Event>> GetOwnEvents (int n, bool NEWEST = true)
		{
			var events = new List<Event> ();

			for (int i = 0; i < n; i++) {
				var url = buildGetRequest ("event", 1);
				JsonValue eventJson = await fetchEvent (url);
				Event e = parseEvent (eventJson);
				events.Add (e);
			}

			return events;
		}

		/// <summary>
		/// Fetchs the event from the HttpRequest.
		/// </summary>
		/// <returns>The event.</returns>
		/// <param name="request">Request to fetch event from.</param>
		private async Task<JsonValue> fetchEvent(HttpWebRequest request)
		{
			//Send the request to the server and await response
			using (WebResponse response = await request.GetResponseAsync ())
			{
				using (Stream stream = response.GetResponseStream ())
				{
					//Build a Json document from the response
					JsonValue jval = await Task.Run( () => JsonObject.Load(stream));

					return jval;
				}
			}
		}

		/// <summary>
		/// Parses the event from a JsonValue.
		/// </summary>
		/// <returns>The parsed event.</returns>
		/// <param name="eventJson">Json-string containing event.</param>
		private Event parseEvent(JsonValue eventJson)
		{
			int id = eventJson ["ID"];
			string name = eventJson ["Name"];
			string description = eventJson ["Description"];
			//Bitmap picture = serverResponse ["Picture"];

			var e = new Event (name, id, description);

			return e;
		}
		#endregion
		/// <summary>
		/// Creates a new user on the server.
		/// </summary>
		/// <param name="u">User to create.</param>
		public void CreateUser (User u)
		{
			//Build request
			var request = buildPostRequest ("User");

			//Serialize and add to request
			string userJson = JsonConvert.SerializeObject (u);
			using (var streamWriter = new StreamWriter (request.GetRequestStream ())) 
			{
				streamWriter.Write (userJson);
				streamWriter.Flush ();
				streamWriter.Close ();
			}

			using (var responseStream = new StreamReader(request.GetResponse ().GetResponseStream())) 
			{
				var result = responseStream.ReadToEnd ();
				//Add code here to notify GUI about potential errors
				//Also set the userID to the one recieved by the response
			}

		}

		/// <summary>
		/// Updates the user specified by id.
		/// </summary>
		/// <param name="i">A reference to a <see cref="DineWithaDane.InfoChange"/> containing Key and Value of the change.</param>
		/// <param name="id">Identifier of the user to change.</param>
		public async void UpdateUser (InfoChange i)
		{
			var request = buildPutRequest ("event", userID);

			var jsonString = JsonConvert.SerializeObject (i);
			using (var streamWriter = request.GetRequestStream())
			{
				//Convert to byte array
				byte[] bytes = stringToByteArray (jsonString);

				//Write the bytearray to stream asyncronized
				await streamWriter.WriteAsync (stringToByteArray(jsonString), 0, bytes.Length);
				await streamWriter.FlushAsync ();
				streamWriter.Close ();
			}

			using (var response = request.GetResponse ())
			{
				//Again some code to notify GUI
			}
		}

		/// <summary>
		/// Deletes the given user.
		/// </summary>
		/// <param name="u">User to delete.</param>
		public void DeleteUser ()
		{
			var request = buildDeleteRequest ("User", userID);

			try
			{
				var response = (HttpWebResponse) request.GetResponse();

				//if (response.StatusCode == HttpStatusCode.OK)
					//Went well i guess?
			}
			catch
			{
				//Code to notify GUI, that user wasn't deleted
			}
		}

		/// <summary>
		/// Creates an event on the server.
		/// </summary>
		/// <param name="e">Event to create.</param>
		public async void CreateEvent (Event e)
		{
			var request = buildPostRequest ("event");

			var eventJson = JsonConvert.SerializeObject (e);

			using (var streamWriter = request.GetRequestStream ())
			{
				byte[] bytes = stringToByteArray (eventJson);

				//Write and flush the stream in the background
				await streamWriter.WriteAsync (bytes, 0, bytes.Length);
				await streamWriter.FlushAsync ();
				streamWriter.Close ();
			}

			using (var responseStream = new StreamReader (request.GetResponse ().GetResponseStream ()))
			{
				await responseStream.ReadToEndAsync ();

				//Again code to notify incase of errors
			}
		}

		public void UpdateEvent ()
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Deletes the given event.
		/// </summary>
		/// <param name="e">Event to delete.</param>
		public void DeleteEvent (Event e)
		{
			var request = buildDeleteRequest ("event", e.id);

			try
			{
				var response = (HttpWebResponse) request.GetResponse();

				//if (response.StatusCode == HttpStatusCode.OK)
				//Went well i guess?
			}
			catch
			{
				//Code to notify GUI, that user wasn't deleted
			}
		}

		public void ReplyOffer (bool a)
		{
			throw new NotImplementedException ();
		}

		public void CancelReg (Event e)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

