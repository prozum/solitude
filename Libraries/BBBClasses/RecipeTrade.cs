using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBBClasses
{
	public class RecipeTrade
	{
		public Guid TradeId { get; set; }

		public Guid SenderBeer { get; set; }
		public Guid SenderUser { get; set; }

		public Guid ReceiverUser { get; set; }
		public Guid ReceiverBeer { get; set; }

		public string Message { get; set; }
	}
}
