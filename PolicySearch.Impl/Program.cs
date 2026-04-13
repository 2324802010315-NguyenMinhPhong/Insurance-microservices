using MediatR;
using Steeltoe.Discovery.Client;
using System.Net.Http.Json;
using PolicySearch.Impl.Models;

var builder = WebApplication.CreateBuilder(args);

// ADD MEDIATR
builder.Services.AddMediatR(typeof(Program));

// ADD EUREKA CLIENT
builder.Services.AddDiscoveryClient(builder.Configuration);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// USE EUREKA
app.UseDiscoveryClient();

app.UseHttpsRedirection();


// =====================
// 🔥 SEARCH ALL POLICY
// =====================
app.MapGet("/search", async () =>
{
    var httpClient = new HttpClient();

    var data = await httpClient.GetFromJsonAsync<List<PolicyModel>>(
        "http://localhost:5128/policy"
    );

    if (data == null)
        return Results.BadRequest("No data from Policy service");

    return Results.Ok(data);
});


// =====================
// 🔥 SEARCH BY NAME
// =====================
app.MapGet("/search/{name}", async (string name) =>
{
    var httpClient = new HttpClient();

    var data = await httpClient.GetFromJsonAsync<List<PolicyModel>>(
        "http://localhost:5128/policy"
    );

    if (data == null)
        return Results.BadRequest("No data");

    var result = data
        .Where(p => p.Customer.ToLower().Contains(name.ToLower()))
        .ToList();

    return Results.Ok(result);
});


// =====================
// TEST API (OPTIONAL)
// =====================
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
});

app.Run();


// Model test (giữ cũng được)
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}