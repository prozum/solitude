using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;

namespace OwinWebApi
{
	public class CustomerController : ApiController
	{
		Customer[] CustomerDB = new Customer[] {
			new Customer { Id = 1, Name = "RMS", Category = "Groceries", Price = 0, Hygiene = -1 },
			new Customer { Id = 2, Name = "Linus Torvalds", Category = "Toys", Price = 1000M, Hygiene = 10 },
			new Customer { Id = 3, Name = "Eric S. Raymond", Category = "Hardware", Price = 500M, Hygiene = 0 },
			new Customer {
				Id = 4,
				Name = "Steve Ballmer",
				Category = "Vegetables",
				Price = -22400000000M,
				Hygiene = -22400000000M
			}
		};

		public IEnumerable<Customer> Get ()
		{
			return CustomerDB;
		}

		public Customer Get (int id)
		{
			var customer = CustomerDB.FirstOrDefault (c => c.Id == id);

			if (customer == null) {
				throw new HttpResponseException (System.Net.HttpStatusCode.NotFound);
			}

			return customer;
		}
	}
}