namespace RecipeAggregatorApi.Models
{
    public class RequestRecipeDTO
    {
        public string Name { get; set; } = null!;
        public string? Content { get; set; }
        public string? Url { get; set; }

    }
}
