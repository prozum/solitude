using System;
using WebBackgrounder;
using Dal;
using System.Threading.Tasks;

namespace Solitude.Server
{
    public class BirthdateJob : Job
    {
        public DatabaseAbstrationLayer Dal;

        public BirthdateJob(string name, TimeSpan time, DatabaseAbstrationLayer dal) : base(name,time)
        {
            Dal = dal;
        }

        public override Task Execute()
        {
            return new Task(() =>
            {
                    Dal.AddBirthdateNotifications(DateTimeOffset.UtcNow.Date);
            });
        }
    }
}

