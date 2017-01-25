using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBBClasses
{
	public class UserData
	{
		public Guid Id { get; private set; }

		public string UserName { private set; get; }

		public string Name { private set; get; }

		public string Location { private set; get; }

		public string Birthdate { private set; get; }
	}
}
