using Azure.Communication.Email;
using EmailSender.Application.Abstractions;
using EmailSender.Infrastructure.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton(_ =>
{
    string? connectionString = builder.Configuration["AzureCommunicationServiceConnection"]
        ?? throw new InvalidOperationException("Azure Communication Service connection string is missing");

    return new EmailClient(connectionString);
});

builder.Services.AddSingleton<IEmailSender, AzureCommunicationServiceEmailSender>();

builder.Build().Run();
