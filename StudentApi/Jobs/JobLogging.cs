using log4net;
using Quartz;

namespace StudentApi.Jobs
{
    [DisallowConcurrentExecution]
    public class JobLogging : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(JobLogging));

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"Job start at: {DateTime.Now}");
            _logger.Info($"Data-1");
            Thread.Sleep(120000);
            _logger.Info($"Data-2");            
            _logger.Info($"Data-3");
            await Task.CompletedTask;
        }
    }
}
