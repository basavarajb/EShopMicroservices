using FluentValidation;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Behaviors;
using Catalog.Api.Data;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
//Add services to container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior( typeof(ValidationBehavior<,>));
    config.AddOpenBehavior( typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Marten"));
}).UseLightweightSessions();
if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Marten"));
var app = builder.Build();
app.MapCarter();
//Configure HTTP request pipeline
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
