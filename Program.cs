using DsaApi.Application.Interfaces;
using DsaApi.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DSA API", Version = "v1" });
}));

// Add Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    // Add other providers like Seq, Application Insights, etc. here if needed
});


// *** Register Application Layer Services ***
// Use AddSingleton for this demo service because it holds state in memory.
// WARNING: This means ONE stack shared by ALL requests. Not suitable for production multi-user scenarios.
// For production, state would likely be external (DB, Cache) and services might be Scoped or Transient.
builder.Services.AddSingleton<IStackService, InMemoryStackService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI((c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DSA API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    }));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
