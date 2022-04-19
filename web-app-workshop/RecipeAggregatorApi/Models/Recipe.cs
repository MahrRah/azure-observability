namespace RecipeAggregatorApi.Models
{
    public class Recipe
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Url { get; set; }
    }
}
