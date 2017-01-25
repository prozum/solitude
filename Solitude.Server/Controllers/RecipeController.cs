using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using BBBClasses;
using System;

namespace Solitude.Server
{
    public class RecipeController : BBBController
    {
        public async Task<IHttpActionResult> Get()
        {
			var events = await DB.GetAllRecipes(UserId);
            return Ok(events);
        }
            
        public async Task<IHttpActionResult> Post(Recipe r)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            r.BrewerId = UserId;

			await DB.AddRecipe(r);

            return Ok(new { Id = r.RecipeId});
        }
            
        public async Task<IHttpActionResult> Delete(Guid id)
        {
			await DB.DeleteRecipe(id);

            return Ok();
        }
            
        public async Task<IHttpActionResult> Put(Recipe r)
        {
            await DB.UpdateRecipe(r);

            return Ok();
        }
    }
}