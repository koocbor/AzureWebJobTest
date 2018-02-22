using AzureWebJobTest.WebJob.Repository;
using AzureWebJobTest.WebJob.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AzureWebJobTest.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var configuration = new JobHostConfiguration();
            configuration.JobActivator = new CustomJobActivator(serviceCollection.BuildServiceProvider());

#if DEBUG
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var functions = serviceProvider.GetService<Functions>();
            var message = "yo";
            using (var task = functions.TestWebJob(message, Console.Out))
            {
                task.Wait();
            }
            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();
#else
            JobHost host = new JobHost(configuration);
            host.RunAndBlock();
#endif
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddDbContext<WidgetDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("connection_db"))
            );

            services.AddScoped<IWidgetRepository, WidgetRepository>();
            services.AddScoped<IWidgetService, WidgetService>();
            services.AddScoped<Functions, Functions>();
        }
    }
}
