namespace RecipeAggregatorApi.Models
{
    public class ResponseRecipeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Content { get; set; }
        public string? Url { get; set; }

        public ResponseRecipeDTO()
        {
            // For EF
        }

        internal ResponseRecipeDTO(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Content = recipe.Content;
            Url = recipe.Url;
        }
    }
}
