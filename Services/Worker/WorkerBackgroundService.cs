// System Usings

// Application Usings
using Worker.Services;

// External Usings


namespace Worker
{

    public sealed class WorkerBackgroundService : BackgroundService
    {
        private readonly WorkerService _workerService;
        private readonly ILogger<WorkerBackgroundService> _logger;

        public WorkerBackgroundService(
            WorkerService workerService,
            ILogger<WorkerBackgroundService> logger) =>
            (_workerService, _logger) = (workerService, logger);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _workerService.Start();

                // Authenticate in the Broker (register or synchronize)
                _workerService.Authenticate();
                _logger.LogInformation($"Authenticated and ready, Id is {_workerService.Worker.UUID}");


                // Lets start listening on RPC
                _workerService.Listen();

                var info = _workerService.Worker.ConnectionInfo;
                _logger.LogInformation($"Worker RPC listening on {info.IPv4}:{info.Port}");

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }

                _workerService.Deafen();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }

}