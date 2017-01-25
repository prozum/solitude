using System.Web.Http;
using System.Threading.Tasks;
using System;
using BBBClasses;

namespace Solitude.Server
{
    public class BeerController : BBBController
    {
        public async Task<IHttpActionResult> Get()
        {
            var beers = await DB.GetBrewedBeers(UserId);
            return Ok(beers);
        }

		public async Task<IHttpActionResult> Post(Beer b)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			b.UserId = UserId;

			await DB.AddBeer(b);

			return Ok(new { Id = b.Id } );
		}

		public async Task<IHttpActionResult> Put(Beer b)
		{
			await DB.UpdateBeer(b, UserId);

			return Ok();
		}

        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await DB.DeleteBeer(id);
            return Ok();
        }
    }
}

