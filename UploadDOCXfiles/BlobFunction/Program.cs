using BlobFunction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLayer.Helpers;
using ServiceLayer.Services;
using ServiceLayer.Services.Abstract;
var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddScoped<IEmailServices, EmailService>();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();


host.Run();