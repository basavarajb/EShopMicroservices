
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Marten")!);
    opt.Schema.For<ShoppingCart>().Identity(x=>x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration=builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Marten")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
var app = builder.Build();
//Configure the services
app.MapGet("/", () => "Hello World!");
app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.MapCarter();
app.UseExceptionHandler(opt => { });
app.Run();
