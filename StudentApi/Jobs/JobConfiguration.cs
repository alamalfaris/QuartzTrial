using Microsoft.Extensions.Options;
using Quartz;

namespace StudentApi.Jobs
{
    public class JobConfiguration : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            //DateTime customeDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0)

			var jobKey = JobKey.Create(nameof(JobLogging));
            options
                .AddJob<JobLogging>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(triger =>
                    triger
                        .ForJob(jobKey)
						//.StartAt(customeDateTime)
						.WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInMinutes(1).RepeatForever()));
        }
    }
}
