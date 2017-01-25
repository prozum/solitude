using System.Web.Http;
using System.Threading.Tasks;
using System;
using BBBClasses;
using System.Collections.Generic;

namespace Solitude.Server
{
    public class TradeController : BBBController
    {
        public async Task<IHttpActionResult> Get()
        {
			var trades = await DB.GetPendingTrades(UserId);
			
            return Ok(trades);
        }
            
        public async Task<IHttpActionResult> Post(RecipeTrade rt, Guid receiver_id)
        {
			rt.SenderUser = UserId;
			await DB.ProposeRecipeTrade(receiver_id, rt);

			//Add a notification
			var r_beer = await DB.GetBeer(rt.ReceiverBeer);
			var s_beer = await DB.GetBeer(rt.SenderBeer);
			var trade_starter = await DB.GetUserData(rt.SenderUser);
			var notification_message = BBBNotificationStrings.TRADE_PENDING(trade_starter, s_beer, r_beer);
			Notification n = new Notification();
			n.Data = new string[]
			{
				notification_message
			};
			await DB.AddNotification(rt.ReceiverUser, n);

            return Ok();
        }

		public async Task<IHttpActionResult> Put(Guid trade_id, bool accepted)
		{
			//Update database
			if (accepted)
			{
				await DB.AcceptOffer(trade_id);
				await addTradeNotificationToSender(trade_id, BBBNotificationStrings.TRADE_ACCEPTED);
			}
			else {
				await DB.DeclineOffer(trade_id);
				await addTradeNotificationToSender(trade_id, BBBNotificationStrings.TRADE_DECLINED);
			}

			return Ok();
		}

        public async Task<IHttpActionResult> Delete(Guid id)
        {
			await DB.DeclineOffer(id);
			await addTradeNotificationToSender(id, BBBNotificationStrings.TRADE_DECLINED);

            return Ok();
        }

		private async Task addTradeNotificationToSender(Guid trade_id, Func<UserData, string> notificationBuilder)
		{
			var trade = await DB.GetTrade(trade_id);
			Notification n = new Notification();
			var receiver = await DB.GetUserData(trade.ReceiverUser);

			string notification = notificationBuilder(receiver);

			//And add the notification
			n.Data = new string[]
			{
				notification
			};
			await DB.AddNotification(trade.SenderUser, n);
		}
	}
}