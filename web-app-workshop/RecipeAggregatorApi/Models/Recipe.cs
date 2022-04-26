namespace RecipeAggregatorApi.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Content { get; set; }
        public string? Url { get; set; }
    }
}
