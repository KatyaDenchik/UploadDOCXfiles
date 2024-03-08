using BlobFunction;
using Microsoft.Extensions.Hosting;
BlobFunction.BlobFunction.SendMail();
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();