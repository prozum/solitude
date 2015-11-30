using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Model
{
    public class Event
	{
		public Guid Id { get; set; }

		[Required]
		public DateTimeOffset Date { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[JsonIgnore]
		public Guid UserId { get; set; }

		[Required]
		public int SlotsTotal { get; set; }

        [JsonIgnore]
        public int SlotsTaken { get; set; }
	}
}

