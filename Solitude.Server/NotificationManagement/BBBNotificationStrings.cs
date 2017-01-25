using System;
using BBBClasses;

namespace Solitude.Server
{
	static class BBBNotificationStrings
	{
		public static string REVIEW_NOTIFICATION(Beer b)
		{
			return String.Format("{0} has been review, check it out!", b.Name);
		}

		public static string TRADE_DECLINED (UserData trade_receiver)
		{
			return String.Format("{0} has declined your trade.", trade_receiver.Name);
		}
		public static string TRADE_ACCEPTED(UserData trade_receiver) {
			return String.Format("{0} has accepted your trade.", trade_receiver.Name);
		}
		public static string TRADE_PENDING(UserData trade_starter, Beer s_beer, Beer r_beer) {
			return String.Format("{0} wants to trade {1} for {2}", trade_starter.Name, s_beer.Name, r_beer.Name);
		}
	}
}
