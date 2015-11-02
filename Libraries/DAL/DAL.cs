using System;
using System.Collections;
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

		static void AddEvent (string uid, Event e)
		{
			client.Cypher
				.Match ("(user:User)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				//creates a relation "HOSTING" between the created event 
				.Create ("user-[:HOSTING]->(event:Event {info})")
				.WithParam ("info", e)
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

		public static IEnumerable MatchUser (string uid, int limit)
		{
			var res = client.Cypher
				//select all users in the system and all the users who hosts an event
				.Match ("(user:User), (rest:User)-[:HOSTING]->(event:Event)")
				//filter out everyone except the user being matched
				.Where ("user.Id = {uid}")
				//filter out the user in the rest of the users who hosts event
				.AndWhere ("rest.Id <> {uid}")
				.WithParam ("uid", uid)
				//match on interests and make sure the data is available to the next clause
				.Match ("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
				.With ("user, rest, event sum(w1.weight) as weight1, sum(w2.weight) as weight2")
				//match on languages and make sure the data is available to the next clause
				.Match ("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
				.With ("user, rest, event, weight1, weight2, sum(w3.weight) as weight3, sum(w4.weight) as weight4")
				//match on foodhabits and make sure the data is available to the next clause
				.Match ("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
				.With ("user, rest, event, weight1, weight2, weight3, weight4," +
					"\tsum(w5.weight) as weight5, sum(w6.weight) as weight6")
				//return a collection of anonymouse types
				.Return (() => new {
					people = Return.As<string> ("rest.Id"),
					score = Return.As<int> (
						"sum(weight1)+sum(weight2) +" +
						"sum(weight3)+sum(weight4) +" +
						"sum(weight5)+sum(weight6)"
					),
					events = Return.As<Event> ("event")
				})
				//order by descending values
				.OrderBy ("value DESC")
				//limit the collection to the given limit
				.Limit (limit)
				.ResultsAsync.Result;

			return res;
		}
	}
}