using Microsoft.Extensions.Options;
using Quartz;

namespace StudentApi.Jobs
{
    public class JobConfiguration : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(JobLogging));
            options
                .AddJob<JobLogging>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(triger =>
                    triger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(5).RepeatForever()));
        }
    }
}
