using FastEndpoints;
using PaintMixer.Api.Diagnostics;
using PaintMixer.Api.Middleware;
using PaintMixer.Service;
using PaintMixer.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDiagnosticMessageProvider, DiagnosticMessageProvider>();
builder.Services.AddSingleton<IPaintMixerDevice, PaintMixerDeviceAdapter>();
builder.Services.AddScoped<IPaintMixerService, PaintMixerService>();

builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseMiddleware<Observability>();

app.UseFastEndpoints();

app.Run();