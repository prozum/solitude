using System;
using WebBackgrounder;
using Dal;
using System.Threading.Tasks;

namespace Solitude.Server
{
    public class EventEndedJob : Job
    {
        public DatabaseAbstrationLayer Dal;

        public EventEndedJob(string name, TimeSpan time, DatabaseAbstrationLayer dal) : base(name,time)
        {
            Dal = dal;
        }

        public override Task Execute()
        {
            return new Task(() => 
                {
                    Dal.DeleteHeldEvents(DateTimeOffset.UtcNow);
                });
        }
    }
}

