using log4net;
using Quartz;

namespace StudentApi.Jobs
{
    [DisallowConcurrentExecution]
    public class JobLogging : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(JobLogging));

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"{DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
