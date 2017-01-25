using System;
using WebBackgrounder;
using Dal;
using System.Threading.Tasks;

namespace Solitude.Server
{
	class CleanJob : Job
	{
		public DatabaseAbstrationLayer Dal;

		public CleanJob (string name, TimeSpan time, DatabaseAbstrationLayer dal) : base(name, time)
		{
			Dal = dal;
		}

		public override Task Execute()
		{
			return new Task(async () =>
			{
				await Dal.CleanUnusedFields();
			});
		}
	}
}
