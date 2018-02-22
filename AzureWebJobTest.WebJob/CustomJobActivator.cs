using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzureWebJobTest.WebJob
{
    public class CustomJobActivator : IJobActivator
    {
        private readonly IServiceProvider serviceProvider;

        public CustomJobActivator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T CreateInstance<T>()
        {
            var service = serviceProvider.GetService<T>();
            return service;
        }
    }
}
