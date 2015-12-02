using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Model
{
    public class Event
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }

		[Required]
		public DateTimeOffset Date { get; set; }

		[Required]
		public string Location { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public int SlotsTotal { get; set; }
		public int SlotsTaken { get; set; }
	}
}

