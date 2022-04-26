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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .ToContainer(nameof(Recipes))
                .HasNoDiscriminator()
                .HasPartitionKey(r => r.Id);
        }
    }
}
