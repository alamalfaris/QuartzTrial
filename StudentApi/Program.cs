using log4net.Config;
using log4net;
using Quartz;
using StudentApi.Jobs;
using System.Reflection;
using StudentApi.Database;
using Microsoft.EntityFrameworkCore;
using CrystalQuartz.AspNetCore;

namespace StudentApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
            });
            builder.Services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            var scheduler = JobConfig.Create().Result;
            //builder.Services.ConfigureOptions<JobConfiguration>()
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();

            app.UseCrystalQuartz(() => scheduler);

            app.Run();
        }
    }
}