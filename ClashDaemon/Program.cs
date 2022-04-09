using ClashDaemon;
using ClashDaemon.ClashLog;
using ClashDaemon.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;

        services.Configure<ExecuteOptions>(
            config.GetSection(nameof(ExecuteOptions))
        );

        services.AddSingleton<IClashLogService, ClashLogService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
