using Microsoft.EntityFrameworkCore;

namespace RecipeAggregatorApi.Models
{
    public class RecipeContext: DbContext
    {
        public RecipeContext(DbContextOptions<RecipeContext> options)
           : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; } = null!;
    }
}
