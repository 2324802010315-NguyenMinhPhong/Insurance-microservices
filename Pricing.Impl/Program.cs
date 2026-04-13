using MediatR;
using Steeltoe.Discovery.Client;
using Pricing.Impl.Features.Pricing;

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
// 🔥 PRICING API (TASK 1)
// =====================
app.MapGet("/pricing", async (int age, string product, IMediator mediator) =>
{
    var result = await mediator.Send(new CalculatePriceQuery(age, product));
    return Results.Ok(result);
});


// =====================
// (OPTIONAL) TEST API
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


// Model (giữ lại cũng được, không bắt buộc)
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}