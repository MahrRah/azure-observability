#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAggregatorApi.Models;

namespace RecipeAggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RecipesController : ControllerBase
    {
        private const string _partitionKey = Recipe.PartitionKeyValue;
        private readonly RecipeContext _context;

        public RecipesController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResponseRecipeDTO>>> GetRecipes()
        {
            return await _context.Recipes.WithPartitionKey(_partitionKey).Select(r => new ResponseRecipeDTO(r)).ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseRecipeDTO>> GetRecipe(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id, _partitionKey);
            if (recipe == null)
            {
                return NotFound();
            }

            return new ResponseRecipeDTO(recipe);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRecipe(Guid id, RequestRecipeDTO recipe)
        {
            var existing = await _context.Recipes.FindAsync(id, _partitionKey);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = recipe.Name;
            existing.Content = recipe.Content;
            existing.Url = recipe.Url;

            _context.Entry(existing).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Recipe>> PostRecipe(RequestRecipeDTO recipe)
        {
            var dbRecipe = new Recipe(recipe.Name, recipe.Content, recipe.Url);
            _context.Recipes.Add(dbRecipe);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipe), new { id = dbRecipe.Id }, new ResponseRecipeDTO(dbRecipe));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id, _partitionKey);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
