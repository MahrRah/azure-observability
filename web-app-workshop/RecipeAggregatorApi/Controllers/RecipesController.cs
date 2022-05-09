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
        private readonly ILogger _logger;

        public RecipesController(RecipeContext context, ILogger<RecipesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResponseRecipeDTO>>> GetRecipes()
        {
            _logger.LogInformation("Getting all recipes");
            return await _context.Recipes.WithPartitionKey(_partitionKey).Select(r => new ResponseRecipeDTO(r)).ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseRecipeDTO>> GetRecipe(Guid id)
        {
            _logger.LogInformation("Getting recipe {Id}", id);
            var recipe = await _context.Recipes.FindAsync(id, _partitionKey);
            if (recipe == null)
            {
                _logger.LogWarning("Cannot get recipe {Id}, recipe does not exist", id);
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
            _logger.LogInformation("Updating recipe {Id}", id);
            var existing = await _context.Recipes.FindAsync(id, _partitionKey);
            if (existing == null)
            {
                _logger.LogWarning("Cannot get recipe {Id}, recipe does not exist", id);
                return NotFound();
            }

            existing.Name = recipe.Name;
            existing.Content = recipe.Content;
            existing.Url = recipe.Url;

            _context.Entry(existing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Recipe {Id} updated", existing.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update recipe {Id}", existing.Id);
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Recipe>> PostRecipe(RequestRecipeDTO recipe)
        {
            _logger.LogInformation("Creating recipe {Name}", recipe.Name);
            var dbRecipe = new Recipe(recipe.Name, recipe.Content, recipe.Url);

            try
            {
                _context.Recipes.Add(dbRecipe);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Recipe {Name} created", dbRecipe.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create recipe {Name}", dbRecipe.Name);
                throw;
            }

            return CreatedAtAction(nameof(GetRecipe), new { id = dbRecipe.Id }, new ResponseRecipeDTO(dbRecipe));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            _logger.LogInformation("Deleting recipe {Id}", id);
            var recipe = await _context.Recipes.FindAsync(id, _partitionKey);
            if (recipe == null)
            {
                _logger.LogWarning("Cannot get recipe {Id}, recipe does not exist", id);
                return NotFound();
            }

            try
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Recipe {Id} deleted", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete recipe {Id}", id);
                throw;
            }

            return NoContent();
        }
    }
}
