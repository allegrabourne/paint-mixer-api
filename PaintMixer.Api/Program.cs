using PaintMixer.Api.Diagnostics;
using PaintMixer.Api.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;

    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

builder.Services.AddSingleton<IDiagnosticMessageProvider, DiagnosticMessageProvider>();

var app = builder.Build();

app.UseMiddleware<Observability>();

app.Run();
