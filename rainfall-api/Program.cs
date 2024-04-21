using Autofac.Core;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using rainfall_api.Common;
using rainfall_api.Middlewares;
using rainfall_api.Services;
using Serilog;
using System.Reflection;
using System.Text.RegularExpressions;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration);
    });

    builder.Services.AddControllers(options =>
    {
        options
        .Conventions
        .Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(Program).Assembly));
    builder.Services.AddHttpClient("EnvironmentAgencyHttpClient", c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ExternalApis:EnvironmentAgency"] ?? "");
        c.DefaultRequestHeaders.Add("Accept", "*/*");
    });
    builder.Services.AddScoped<ExceptionMiddleware>();
    builder.Services.AddSingleton<RequestLoggingMiddleware>();
    builder.Services.AddScoped<ResponseLoggingMiddleware>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseMiddleware<ExceptionMiddleware>();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value)
    {
        if (value == null) { return null; }

        return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
