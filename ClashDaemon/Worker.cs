using ClashDaemon.ClashLog;
using System.Diagnostics;

namespace ClashDaemon
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IClashLogService _clashLog;

        public Worker(ILogger<Worker> logger, IClashLogService clashLog)
        {
            _logger = logger;
            _clashLog = clashLog;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var process = new Process()
            {
                StartInfo = new("clash.exe")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            process.Start();
            
            process.OutputDataReceived += (_, e)
                => _clashLog.HandleLog(e.Data);

            process.ErrorDataReceived += (s, _e) => _logger.LogError(_e.Data);
            process.Exited += (s, _e) => _logger.LogWarning("Exited!");
            process.EnableRaisingEvents = true;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            process.Kill();
        }
    }
}