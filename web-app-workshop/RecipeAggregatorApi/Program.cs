using Microsoft.EntityFrameworkCore;
using RecipeAggregatorApi;
using RecipeAggregatorApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var cosmosDb = new CosmosDbConfiguration();
builder.Configuration.GetSection(CosmosDbConfiguration.CosmosDb).Bind(cosmosDb);
builder.Services.AddDbContext<RecipeContext>(opt =>
    opt.UseCosmos(
        cosmosDb.AccountEndpoint,
        cosmosDb.AccountKey,
        cosmosDb.DatabaseName));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<RecipeContext>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
