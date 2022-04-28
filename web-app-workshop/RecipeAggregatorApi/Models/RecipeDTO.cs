namespace RecipeAggregatorApi.Models
{
    public class RecipeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Content { get; set; }
        public string? Url { get; set; }

        public RecipeDTO(string name, string? content, string? url)
        {
            Id = Guid.NewGuid();
            Name = name;
            Content = content;
            Url = url;
        }

        internal RecipeDTO(Recipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Content = recipe.Content;
            Url = recipe.Url;
        }
    }
}
