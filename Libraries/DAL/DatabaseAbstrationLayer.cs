using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using Model;

namespace Dal
{
	public class DatabaseAbstrationLayer : IDisposable
	{
		private readonly GraphClient client;

		public DatabaseAbstrationLayer(GraphClient client)
		{
			this.client = client;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Adds an interest to the database
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="i">The interest to add</param>
		public async Task AddInterest (Interest i)
		{
			await client.Cypher
				.Merge("(i:Interest { Id:{id}, Name:{name}})")
				.WithParams(new 
					{
						id = i.Id,
						name = i.Name
					})
				.ExecuteWithoutResultsAsync();
		}

		/// <summary>
		/// Adds a language to the database
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="lang">The language to add</param>
		public async Task AddLanguage (Language lang)
		{
			await client.Cypher
				.Merge("(lang:Language { Id:{id}, Name:{name}})")
				.WithParams(new 
					{
						id = lang.Id,
						name = lang.Name
					})
				.ExecuteWithoutResultsAsync();
		}

		/// <summary>
		/// Adds a foodhabit to the database
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="fh">The foodhabit to add</param>
		public async Task AddFoodHabit (FoodHabit fh)
		{
			await client.Cypher
				.Merge("(fb:FoodHabit { Id:{id}, Name:{name}})")
				.WithParams(new 
					{
						id = fh.Id,
						name = fh.Name
					})
				.ExecuteWithoutResultsAsync();
		}

		/// <summary>
		/// Sets the event id counter in the database
		/// Should not be run if the database already
		/// has an event id counter
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="startVal">The value which the event id counter is set to</param>
		public async Task SetEventIdCounter (int startVal)
		{
			await client.Cypher
				.Merge("(sinfo:ServerInfo)")
				.OnCreate()
				.Set("sinfo.EventCounter = {val}")
				.WithParam("val", startVal)
				.ExecuteWithoutResultsAsync();
		}

		/// <summary>
		/// Access the event id counter (the id counter must exist)
		/// </summary>
		/// <returns>The event identifier counter as a Task<int></returns>
		async Task<int> GetEventIdCounter ()
		{
			var res = await client.Cypher
				.Match ("(sinfo:ServerInfo)")
				.Return (() => Return.As<int>("sinfo.EventCounter"))
				.ResultsAsync;

			return res.First ();
		}

		/// <summary>
		/// Increments the event id counter
		/// </summary>
		/// <returns>Task</returns>
		async Task IncrementEventIdCounter ()
		{
			await client.Cypher
				.Match ("(sinfo:ServerInfo)")
				.Set ("sinfo.EventCounter = sinfo.EventCounter + 1")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Add an event to the database
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="event">The event that should be added</param>
		public async Task AddEvent (Event @event)
		{
			@event.Id = await GetEventIdCounter ();
			await IncrementEventIdCounter ();

			await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == @event.UserId)
				//creates a relation "HOSTING" between the created event 
				.Create ("user-[:HOSTING]->(event:Event {info})")
				.WithParam ("info", @event)
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Add a review to the database
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="review">The review that should be added</param>
		public async Task AddReview(Review review)
		{
			await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == review.UserId)
				.Create ("user-[:GAVE_REVIEW]->(review:Review {data})")
				.WithParam ("data", review)
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Connects the user to an interest
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="ic">The interest which the user should be connected to</param>
		/// <param name="w">The weight of the relationship between the user and interest</param>
		public async Task ConnectUserInterest (string uid, int ic, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (interest:Interest)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique ("user-[:WANTS {weight}]->interest")
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Disconnects the user from an interest
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="ic">The interest that the user should be disconnected from</param>
		public async Task DisconnectUserInterest (string uid, int ic)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(interest:Interest)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Gets the user's interests
		/// </summary>
		/// <returns>Task<IEnumerable<int>> with the interest values the user has </returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<int>> GetUserInterest (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:WANTS]->(interest:Interest)")
				.Where ((User user) => user.Id == uid)
				.Return (() => Return.As<int> ("interest.Id"))
				.ResultsAsync;

			return res;
		}

		/// <summary>
		/// Connects the user to a language
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="lc">The language which the user should be connected to</param>
		/// <param name="w">The weight of the relationship between the user and language</param>
		public async Task ConnectUserLanguage (string uid, int lc, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (language:Language)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("language.Id = {lc}")
				.WithParam ("lc", lc)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->language"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Disconnects the user from a language
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="lc">The language that the user should be disconnected from</param>
		public async Task DisconnectUserLanguage (string uid, int lc)
		{
			await client.Cypher
			//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(language:Language)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("language.Id = {lc}")
				.WithParam ("lc", lc)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Gets the user's languages
		/// </summary>
		/// <returns>Task<IEnumerable<int>> with the language values the user has </returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<int>> GetUserLanguage (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:WANTS]->(language:Language)")
				.Where ((User user) => user.Id == uid)
				.Return (() => Return.As<int> ("language.Id"))
				.ResultsAsync;

			return res;
		}

		/// <summary>
		/// Connects the user to a foodhabit
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="fh">The foodhabit which the user should be connected to</param>
		/// <param name="w">The weight of the relationship between the user and foodhabit</param>
		public async Task ConnectUserFoodHabit (string uid, int fh, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (foodhabit:FoodHabit)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("foodhabit.Id = {fh}")
				.WithParam ("fh", fh)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->foodhabit"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Disconnects the user from a foodhabit
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="fh">The foodhabit that the user should be disconnected from</param>
		public async Task DisconnectUserFoodHabit (string uid, int fh)
		{
			await client.Cypher
			//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(foodhabit:FoodHabit)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("foodhabit.Id = {fh}")
				.WithParam ("fh", fh)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Gets the user's foodhabits
		/// </summary>
		/// <returns>Task<IEnumerable<int>> with the foodhabit values the user has </returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<int>> GetUserFoodHabit (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:WANTS]->(foodhabit:FoodHabit)")
				.Where ((User user) => user.Id == uid)
				.Return (() => Return.As<int> ("foodhabit.Id"))
				.ResultsAsync;

			return res;
		}

		/// <summary>
		/// Cleans all matches for a given user
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		async Task CleanMatches (string uid)
		{
			await client.Cypher
				.Match ("(user:User)-[m:MATCHED]->(event:Event)")
				.Where((User user) => user.Id == uid)
				.Delete ("m")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Matchs the user against all users that are hosting an event
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's ID</param>
		/// <param name="LIMIT">The maximum limit of matches created</param>
		public async Task MatchUser(string uid, int LIMIT = 5)
		{
			await CleanMatches (uid);

			/*
			 * 1: select all users (user) and users who host an event
			 * 2: filter out all users in user, except the one the uid is for
			 * 3: make sure the user isn't compared with itself (user shouldn't be in rest)
			 * 4: filter out all the events that don't have slots left
			 * 5: replace the 'uid' with the uid arg
			 * 6: add param uid
			 * 7,8,9,10,11,12: calculate weights for rest against user and send the results further along
			 * 13: order the rest by how great they are matched, descending order
			 * 14: only take the top (LIMIT) of the rest
			 * 15: create 'relationship' "MATCHED" from user to all the events that fits
			 */

			await client.Cypher
				.Match ("(user:User), (rest:User)-[:HOSTING]->(e:Event)")
				.Where ((User user) => user.Id == uid )
				.AndWhere ((User rest) => rest.Id != uid)
				.AndWhere ((Event e) => e.SlotsTotal > e.SlotsTaken)
				.OptionalMatch ("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
				.With ("user, rest, e, sum(w1.weight) + sum(w2.weight) as wt1")
				.OptionalMatch ("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
				.With ("user, rest, e, wt1, sum(w3.weight) + sum(w4.weight) as wt2")
				.OptionalMatch ("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
				.With ("user, e, wt1, wt2, sum(w5.weight) + sum(w6.weight) as wt3")
				.OrderBy ("(wt1+wt2+wt3) DESC")
				.Limit (LIMIT)
				.Create ("user-[m:MATCHED]->e")
				.ExecuteWithoutResultsAsync ();

			/*
			if (res.First().matches > 0)
			{
				await AddNotification (uid, "You have new offers pending");
			}
			*/
		}

		/// <summary>
		/// Gets the events a user has been offered
		/// </summary>
		/// <returns>Task<IEnumerable<Event>> with the offers(events)</returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<Event>> GetOffers (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:MATCHED]->(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return (() => Return.As<Event> ("event"))
				.ResultsAsync;

			return res;
		}

		/// <summary>
		/// Gets the events that a user is hosting
		/// </summary>
		/// <returns>Task<IEnumerable<Event>> with the events the user is hosting</returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<Event>> GetHostingEvents (string uid)
		{
			var hosting = await client.Cypher
				.Match ("(user:User)-[:HOSTING]->(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return (() => Return.As<Event> ("event"))
				.ResultsAsync;

			return hosting;
		}

		/// <summary>
		/// Gets the events that a user is attending
		/// </summary>
		/// <returns>Task<IEnumerable<Event>> with the events the user is attending</returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<Event>> GetAttendingEvents (string uid)
		{
			var attending = await client.Cypher
				.Match ("(user:User)-[:ATTENDS]->(event:Event)")
				.Where ((User user) => user.Id == uid)
				.Return (() => Return.As<Event> ("event"))
				.ResultsAsync;

			return attending;
		}

		/// <summary>
		/// Updates an event
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="event">The event which replaces the old event</param>
		public async Task UpdateEvent (Event @event)
		{
			await client.Cypher
				.Match ("user-[:HOSTING]->(event:Event)")
				.Where ("event.Id = {eid}")
				.WithParam ("eid", @event.Id)
				.Set ("event = {newinfo}")
				.WithParam ("newinfo", @event)
				.ExecuteWithoutResultsAsync();
		}

		/// <summary>
		/// Deletes an event
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="eid">The event's id</param>
		public async Task DeleteEvent (int eid)
		{
			await client.Cypher
				.OptionalMatch ("(e:Event)<-[r]-()")
				.Where ((Event e) => e.Id == eid)
				.Delete ("e, r")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Take a slot in an event
		/// </summary>
		/// <returns>Task<bool> whether or not it's possible to take the slot </returns>
		/// <param name="eid">The event's id</param>
		public async Task<bool> TakeSlot(int eid)
		{
			var res = await client.Cypher
				.Match ("(e:Event)")
				.Where ((Event e) => e.Id == eid)
				.AndWhere ((Event e) => e.SlotsTotal > e.SlotsTaken)
				.Set ("e.SlotsTaken = e.SlotsTaken + 1")
				.Return ((e) => e.As<Event> ())
				.ResultsAsync;

			return res.Any();
		}

		/// <summary>
		/// Release a slot in an event
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="eid">The event's id</param>
		public async Task ReleaseSlot(int eid)
		{
			await client.Cypher
				.Match("(e:Event)")
				.Where((Event e) => e.Id == eid)
				.AndWhere((Event e) => e.SlotsTaken > 0)
				.Set("e.SlotsTaken = e.SlotsTaken - 1")
				.ExecuteWithoutResultsAsync();
		}

		/// <summary>
		/// Cancels the registration for a user to an event
		/// </summary>
		/// <param name="uid">The user's id</param>
		/// <param name="eid">The event's id</param>
		public async Task CancelRegistration (string uid, int eid)
		{
			await client.Cypher
				.Match ("(user:User)-[a:ATTENDS]->(e:Event)")
				.Where((User user) => user.Id == uid)
				.AndWhere((Event e) => e.Id == eid)
				.Delete ("a")
				.ExecuteWithoutResultsAsync();

			await ReleaseSlot(eid);

			//await AddNotification (@event.UserID, "A person has cancelled his/her registration for your event.");
		}

		/// <summary>
		/// Reply to an offer
		/// </summary>
		/// <returns>Returns a Task<bool> for whether it succeeds or not</returns>
		/// <param name="uid">The user's id</param>
		/// <param name="answer">true if the user wants to attend the event and false if vice versa</param>
		/// <param name="eid">The event's id</param>
		public async Task<bool> ReplyOffer (string uid, bool answer, int eid)
		{
			if (answer)
			{
				var freeSlots = await TakeSlot(eid);

				if (!freeSlots)
					return false;

				await client.Cypher
					.Match ("(user:User)-[m:MATCHED]->(e:Event)")
					.Where ((User user) => user.Id == uid)
					.AndWhere ((Event e) => e.Id == eid)
					.CreateUnique ("user-[:ATTENDS]->e")
					.Delete ("m")
					.ExecuteWithoutResultsAsync();

				return true;
			}
			else
			{
				await client.Cypher
					.Match ("(user:User)-[m:MATCHED]->(e:Event)")
					.Where((User user) => user.Id == uid)
					.Where((Event e) => e.Id == eid)
					.Delete ("m")
					.ExecuteWithoutResultsAsync();

				return true;
			}
		}

		public async Task<UserData> GetUserData(string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == uid)
				.Return (() => Return.As<UserData> ("user"))
				.ResultsAsync;

			return res.First();
		}

		/*
		public async Task AddNotification (string uid, string msg)
		{
			await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == uid)
				.Create ("user-[:HAS]->(notification:Notification {msg})")
				.WithParam ("msg", msg)
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ClearNotification (string uid)
		{
			await client.Cypher
				.Match ("(user:User)-[h:HAS]-(notification:Notification)")
				.Where((User user) => user.Id == uid)
				.Delete ("h, notification")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task<IEnumerable<string>> GetNotification (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:HAS]-(notification:Notification)")
				.Where((User user) => user.Id == uid)
				.Return ((notifications) => Return.As<IEnumerable<string>> ("collect(notification)"))
				.ResultsAsync;

			await ClearNotification (uid);

			return res.First();
		}
		*/

		/// <summary>
		/// Deletes the user's data, used as a help function for user deletion
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		public async Task DeleteUserData(string uid)
		{
			var hostIds = await client.Cypher
				.Match("(user:User)-[:HOSTS]-(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return((@event) => @event.As<Event>().Id)
				.ResultsAsync;

			foreach (var id in hostIds)
			{
				await DeleteEvent(id);
			}

			var attendingIds = await client.Cypher
				.Match("(user:User)-[:ATTENDS]-(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return((@event) => @event.As<Event>().Id)
				.ResultsAsync;

			foreach (var id in attendingIds)
			{
				await CancelRegistration (uid, id);
			}

			await client.Cypher
				.OptionalMatch ("(user:User)-[r]->()")
				.Where((User user) => user.Id == uid)
				.Delete ("r")
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// Deletes a user
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		public async Task DeleteUser(string uid)
		{
			await client.Cypher
				.Match("(user:User)")
				.Where((User user) => user.Id == uid)
				.Delete("user")
				.ExecuteWithoutResultsAsync();
		}
	}
}