using ClashDaemon.ClashLog;
using System.Diagnostics;
using System.Text;

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
                    StandardOutputEncoding = Encoding.UTF8
                },
                EnableRaisingEvents = true
            };

            process.Start();
            
            process.OutputDataReceived += (_, e)
                => _clashLog.HandleLog(e.Data);
            process.ErrorDataReceived += (_, e)
                =>  _logger.LogError(e.Data);
            process.Exited += (s, _e) => _logger.LogWarning("Exited!");

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(stoppingToken);
        }
    }
}