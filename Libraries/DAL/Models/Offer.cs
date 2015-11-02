using System;

namespace DAL
{
	public class Offer : Event
	{
		public int eid;
		public string uid;

		public Offer (int eid, string uid, Event e) : base (e)
		{
			this.eid = eid;
			this.uid = uid;
		}
	}
}

