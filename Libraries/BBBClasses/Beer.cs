using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBBClasses
{
    public class Beer
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid UserId { get; set; }

		public float ABV { get; set; }
		public int EBC { get; set; }
		public int IBU { get; set; }
    }
}
