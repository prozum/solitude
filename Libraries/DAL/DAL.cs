using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace DAL
{
	public static class DAL
	{
		// client has to be set manually, and NEEDS to be set
		public static GraphClient client;

		/// <summary>
		/// function to add an interest to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of interests
		/// </summary>
		/// <returns><c>true</c>, if interest was added, <c>false</c> otherwise.</returns>
		/// <param name="ic">Ic.</param>
		public static bool AddInterest (Interest.InterestCode ic)
		{
			string interest = Interest.GetInterest (ic);

			if (interest == null) {
				return false;
			} else {
				client.Cypher
					.Create ("(interest:Interest {info})")
					.WithParam ("info", new {Id = ic, Description = interest})
					.ExecuteWithoutResultsAsync ();
			}

			return true;
		}
		/// <summary>
		/// function to add a language to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of languages
		/// </summary>
		/// <returns><c>true</c>, if language was added, <c>false</c> otherwise.</returns>
		/// <param name="lc">Lc.</param>
		public static bool AddLanguage (Language.LanguageCode lc)
		{
			string language = Language.GetLanguage (lc);

			if (language == null) {
				return false;
			} else {
				client.Cypher
				.Create ("(language:Language {info})")
				.WithParam ("info", new {Id = lc, Description = language})
				.ExecuteWithoutResultsAsync ();
			}

			return true;
		}

		/// <summary>
		/// function to add a foodhabit to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of foodhabits
		/// </summary>
		/// <returns><c>true</c>, if food habit was added, <c>false</c> otherwise.</returns>
		/// <param name="fc">Fc.</param>
		public static bool AddFoodHabit (FoodHabit.FoodHabitCode fc)
		{
			string foodhabit = FoodHabit.GetFoodHabit (fc);

			if (foodhabit == null) {
				return false;
			} else {
				client.Cypher
					.Create ("(foodhabit:FoodHabit {info})")
					.WithParam ("info", new {Id = fc, Description = foodhabit})
					.ExecuteWithoutResultsAsync ();
			}

			return true;
		}

		static void AddEvent (string uid, string date, string description, string address)
		{
			//get largest id for events owned by the user
			var id = client.Cypher
				.Match ("(user:User)-[:HOSTING]->(event:Event)")
				.Return (() => new {
					id = Return.As<int> ("max(event.Id)")
				})
				.ResultsAsync.Result;

			//increment id by one
			int eid = id.GetEnumerator ().Current.id + 1;

			client.Cypher
				.Match ("(user:User)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				//creates a relation "HOSTING" between the created event 
				.Create ("user-[:HOSTING]->(event:Event {info})")
				.WithParam ("info", new Event (date, address, description, uid, eid))
				.ExecuteWithoutResultsAsync ();
		}
			
		public static void ConnectUserInterest (string uid, Interest.InterestCode ic, int w)
		{
			client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (interest:Interest)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public static void ConnectUserLanguage (string uid, Language.LanguageCode lc, int w)
		{
			client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (language:Language)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id == {lc}")
				.WithParam ("lc", lc)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public static void ConnectUserLanguage (string uid, FoodHabit.FoodHabitCode fc, int w)
		{
			client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (foodhabit:FoodHabit)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id == {fc}")
				.WithParam ("fc", fc)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public static void MatchUser (string uid, int LIMIT = 5)
		{
			client.Cypher
			    //select all users in the system and all the users who hosts an event
				.Match ("(user:User), (rest:User)-[:HOSTING]->(event:Event)")
			    //filter out everyone except the user being matched
				.Where ("user.Id = {uid}")
			    //filter out the user in the rest of the users who hosts event
				.AndWhere ("rest.Id <> {uid}")
				.WithParam ("uid", uid)
			    //match on interests and make sure the data is available to the next clause
				.Match ("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
				.With ("user, rest, event, sum(w1.weight) + sum(w2.weight) as wt1")
			    //match on languages and make sure the data is available to the next clause
				.Match ("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
				.With ("user, rest, event, wt1, sum(w3.weight) + sum(w4.weight) as wt2")
			    //match on foodhabits and make sure the data is available to the next clause
				.Match ("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
				.With ("user, event, wt1, wt2, sum(w5.weight) + sum(w6.weight) as wt3")
				.OrderBy ("(wt1+wt2+wt3) DESC")
				.Limit (LIMIT)
				.Create ("user-[:MATCHED]->event")
				.ExecuteWithoutResultsAsync ();
		}

		public static IEnumerable<Event> GetOffers (string uid)
		{
			var res = client.Cypher
				.Match ("(user:User)-[:MATCHED]->(event:Event)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Return (() => new {
					offers = Return.As<IEnumerable<Event>> ("collect(event)")
				})
				.ResultsAsync.Result;

			return res.First ().offers;
		}

		public static IEnumerable<Event> GetEvents (string uid, bool ATTENDING = true, int LIMIT = 10)
		{
			if (ATTENDING)
			{
				var res = client.Cypher
					.Match ("(user:User)-[:ATTENDING]->(event:Event)")
					.Where ("user.Id = {uid}")
					.WithParam ("uid", uid)
					.Return (() => new {
						attending = Return.As<IEnumerable<Event>> ("collect(event)")
					})
					.Union ()
					.Match ("user-[:HOSTING]->(event:Event)")
					.Where ("user.Id = {uid}")
					.WithParam ("uid", uid)
					.Return (() => new {
						events = Return.As<IEnumerable<Event>> ("collect(event)")
					})
					.Limit (LIMIT)
					.ResultsAsync.Result;

				return res.First ().events;
			}
			else
			{
				var res = client.Cypher
					.Match ("(user:User)-[:HOSTING]->(event:Event)")
					.Where ("user.Id = {uid}")
					.WithParam ("uid", uid)
					.Return (() => new {
						hosting = Return.As<IEnumerable<Event>> ("collect(event)")
					})
					.Limit (LIMIT)
					.ResultsAsync.Result;

				return res.First ().hosting;
			}
		}

		public static void UpdateEvent (string uid, Event e)
		{
			client.Cypher
				.Match ("user-[:HOSTING]->(event:Event {info})")
				.Where ("user.Id = {uid}")
				.AndWhere ("event.Id = eid AND event.uid = uid")
				.WithParam ("uid", uid)
				.WithParam ("eid", e.eid)
				.Set ("info = {newinfo}")
				.WithParam ("newinfo", e)
				.ExecuteWithoutResultsAsync ();
		}

		public static void DeleteEvent (string uid, int eid)
		{
			client.Cypher
				.Match ("user-[:HOSTING]->(event:Event)<-[r]-()")
				.WithParam ("user.Id", uid)
				.Delete ("r, event")
				.ExecuteWithoutResultsAsync ();
		}

		public static void CancelRegistration (string uid, Event e)
		{
			client.Cypher
				.Match ("(user:User)-[a:ATTENDS]->(event:Event)")
				.Where ("user.Id = {uid} AND event.uid = {euid} AND event.eid = {eid}")
				.WithParam ("uid", uid)
				.WithParam ("euid", e.uid)
				.WithParam ("eid", e.eid)
				.Delete ("a")
				.ExecuteWithoutResultsAsync ();
		}
	}
}