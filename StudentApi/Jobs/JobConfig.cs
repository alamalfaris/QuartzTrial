using log4net;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using StudentApi.Database;

namespace StudentApi.Jobs
{
    public static class JobConfig
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(JobConfig));
        private static readonly DatabaseContext _context = new();

        public static async Task<IScheduler> Create()
        {
            _logger.Info("Method: Create");

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            var jobs = await _context.MasterScheduler.ToListAsync();
            //int index = 0
            foreach (var job in jobs)
            {
                var jobKey = new JobKey(job.JobName);

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(job.JobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInMinutes(job.IntervalJob).RepeatForever())
                    .Build();

                Type type = Type.GetType("StudentApi.Jobs." + job.JobName)!;
                var jobDetail = JobBuilder.Create(type)
                        .WithIdentity(jobKey)
                        .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }

            _logger.Info("Create success");
            await scheduler.Start();
            return scheduler;
        }
    }
}
