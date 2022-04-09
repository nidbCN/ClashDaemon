using ClashDaemon.ClashLog;
using ClashDaemon.Options;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text;

namespace ClashDaemon;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IClashLogService _clashLog;
    private readonly ExecuteOptions _options;

    public Worker(ILogger<Worker> logger,
        IClashLogService clashLog,
        IOptions<ExecuteOptions> options)
    {
        _logger = logger;
        _clashLog = clashLog;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var process = new Process()
        {
            StartInfo = new(_options.ClashPath)
            {
                Arguments = _options.ClashArguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8
            },
            EnableRaisingEvents = true
        };

        process.Start();

        process.OutputDataReceived += (_, e)
            => _clashLog.HandleLog(e.Data);
        process.ErrorDataReceived += (_, e)
            => _logger.LogError("stderr: {err}",e.Data);

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }

        process.Kill();
    }
}
