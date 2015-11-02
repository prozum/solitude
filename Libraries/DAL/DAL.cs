using System;
using System.Collections;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace DAL
{
	public static class DAL
	{
		static GraphClient client = new GraphClient (new Uri ("http://prozum.dk:7474/db/data"), "neo4j", "password");

		static bool AddInterest (Interest.InterestCode ic)
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

		static bool AddLanguage (Language.LanguageCode lc)
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

		static bool AddFoodHabit (FoodHabit.FoodHabitCode fc)
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

		static void AddEvent (int uid, Event e)
		{
			client.Cypher
				.Match ("(user:User)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Create ("user-[:HOSTING]->(event:Event {info})")
				.WithParam ("info", e)
				.ExecuteWithoutResultsAsync ();
		}
			
		static void ConnectUserInterest (string uid, Interest.InterestCode ic, int w)
		{
			client.Cypher
				.Match ("(user:User), (interest:Interest)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		static void ConnectUserLanguage (string uid, Language.LanguageCode lc, int w)
		{
			client.Cypher
				.Match ("(user:User), (language:Language)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id == {lc}")
				.WithParam ("lc", lc)
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		static void ConnectUserLanguage (string uid, FoodHabit.FoodHabitCode fc, int w)
		{
			client.Cypher
				.Match ("(user:User), (foodhabit:FoodHabit)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id == {fc}")
				.WithParam ("fc", fc)
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		static IEnumerable MatchUser (string uid, int limit)
		{
			var res = client.Cypher
				.Match ("(user:User), (rest:User)-[:HOSTING]->(event:Event)")
				.Where ("user.Id = {uid}")
				.AndWhere ("rest.Id <> {uid}")
				.WithParam ("uid", uid)
				.Match ("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
				.With ("user, rest, event sum(w1.weight) as weight1, sum(w2.weight) as weight2")
				.Match ("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
				.With ("user, rest, event, weight1, weight2, sum(w3.weight) as weight3, sum(w4.weight) as weight4")
				.Match ("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
				.With ("user, rest, event, weight1, weight2, weight3, weight4, sum(w5) as weight5, sum(w6) as weight6")
				.Return (() => new {
					people = Return.As<string> ("rest.Id"),
					value = Return.As<int> (
						"sum(weight1)+sum(weight2) +" +
						"sum(weight3)+sum(weight4) +" +
						"sum(weight5)+sum(weight6)"
					),
					events = Return.As<Event> ("event")
				})
				.OrderBy ("value DESC")
				.Limit (limit)
				.ResultsAsync.Result;

			return res;
		}
	}
}

