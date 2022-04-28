namespace RecipeAggregatorApi.Models
{
    public class Recipe
    {
        public const string PartitionKeyValue = "Recipe";

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Content { get; set; }
        public string? Url { get; set; }
        public string PartitionKey { get; set; } = PartitionKeyValue;

        public Recipe(Guid id, string name, string? content, string? url)
        {
            Id = id;
            Name = name;
            Content = content;
            Url = url;
        }

        internal Recipe(RecipeDTO recipe)
            : this(recipe.Id, recipe.Name, recipe.Content, recipe.Url)
        {
        }
    }
}
