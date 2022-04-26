namespace RecipeAggregatorApi
{
    public class CosmosDbConfiguration
    {
        public const string CosmosDb = "CosmosDb";
        public string AccountEndpoint { get; set; } = null!;
        public string AccountKey { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}