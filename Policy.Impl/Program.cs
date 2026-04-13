using MediatR;
using Steeltoe.Discovery.Client;
using Policy.Impl.Features.Policy;

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
// 🔥 POLICY APIs
// =====================

// CREATE POLICY
app.MapPost("/policy", (PolicyModel policy) =>
{
    if (policy.Price <= 0)
        return Results.BadRequest("Price must > 0");

    // Auto tăng ID
    var id = FakeDb.Policies.Count + 1;
    var newPolicy = policy with { Id = id };

    FakeDb.Policies.Add(newPolicy);

    return Results.Ok(newPolicy);
});


// GET ALL POLICY
app.MapGet("/policy", () =>
{
    return Results.Ok(FakeDb.Policies);
});


// GET POLICY BY ID (bonus ăn điểm)
app.MapGet("/policy/{id}", (int id) =>
{
    var policy = FakeDb.Policies.FirstOrDefault(p => p.Id == id);

    if (policy == null)
        return Results.NotFound();

    return Results.Ok(policy);
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


// Model (không quan trọng, giữ cũng được)
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}