using Quartz;

namespace StudentApi.Jobs
{
    [DisallowConcurrentExecution]
    public class JobLogging : IJob
    {
        private readonly ILogger<JobLogging> _logger;

        public JobLogging(ILogger<JobLogging> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{UtcNow}", DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}
