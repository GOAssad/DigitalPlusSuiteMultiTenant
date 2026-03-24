using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DigitalPlus.Provisioning.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<CompanyNameSanitizer>();
builder.Services.AddSingleton<ActivationCodeService>();
builder.Services.AddSingleton<DatabaseProvisioningService>();
builder.Services.AddSingleton<LicenseService>();
builder.Services.AddSingleton<LemonSqueezyService>();

builder.Build().Run();
