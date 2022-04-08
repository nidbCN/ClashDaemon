using ClashDaemon;
using ClashDaemon.ClashLog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IClashLogService, ClashLogService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
