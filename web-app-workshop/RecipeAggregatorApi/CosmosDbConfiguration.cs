namespace RecipeAggregatorApi
{
    public class CosmosDbConfiguration
    {
        public const string CosmosDb = "CosmosDb";
        public string AccountEndpoint { get; set; } = String.Empty;
        public string AccountKey { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}