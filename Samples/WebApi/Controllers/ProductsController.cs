using WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "RMS", Category = "Groceries", Price = 0, Hygiene= -1 },
            new Product { Id = 2, Name = "Linus Torvalds", Category = "Toys", Price = 1000M, Hygiene = 10 },
            new Product { Id = 3, Name = "Eric S. Raymond", Category = "Hardware", Price = 500M, Hygiene = 0 },
            new Product { Id = 3, Name = "Steve Ballmer", Category = "Vegetables", Price = -22400000000M, Hygiene=-22400000000M}
        };

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(string name)
        {
            var product = products.FirstOrDefault((p) => p.Name.Contains(name));
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
