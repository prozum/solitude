using System;
using Dal;
using Neo4jClient;
using System.Configuration;
using NUnit.Framework;

namespace Solitude.Server.Tests
{
	//[TestFixture ()]
	public class DalTests
	{
		public DatabaseAbstrationLayer Dal;
		public GraphClient Client;
		
		public DalTests ()
		{
			Client = new GraphClient(new Uri(ConfigurationManager.ConnectionStrings ["neo4j"].ConnectionString));
			Client.Connect ();
			Dal = new DatabaseAbstrationLayer (Client);
		}

		[Test]
		public void TestAddBirthdateNotifications()
		{
			Dal.AddBirthdateNotifications(DateTimeOffset.UtcNow.Date);
		}

		[Test]
		public void TestDeleteHeldEvents()
		{
			Dal.DeleteHeldEvents (DateTimeOffset.UtcNow);
		}
	}
}

