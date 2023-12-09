using log4net;
using Quartz;

namespace StudentApi.Jobs
{
    [DisallowConcurrentExecution]
    public class JobThree : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(JobThree));

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"JobThree");

            return Task.CompletedTask;
        }
    }
}
