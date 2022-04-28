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
        public async Task<ActionResult<IEnumerable<RecipeDTO>>> GetRecipes()
        {
            return await _context.Recipes.WithPartitionKey(_partitionKey).Select(r => new RecipeDTO(r)).ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecipeDTO>> GetRecipe(Guid id)
        {
            var recipe = await _context.Recipes.FindAsync(id, _partitionKey);
            if (recipe == null)
            {
                return NotFound();
            }

            return new RecipeDTO(recipe);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRecipe(Guid id, RecipeDTO recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(new Recipe(recipe)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Recipe>> PostRecipe(RecipeDTO recipe)
        {
            _context.Recipes.Add(new Recipe(recipe));
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
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

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.WithPartitionKey(_partitionKey).Where(e => e.Id == id).Count() > 0;
        }
    }
}
