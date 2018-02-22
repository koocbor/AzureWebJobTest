using AzureWebJobTest.WebJob.Services;
using Microsoft.Azure.WebJobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureWebJobTest.WebJob
{
    public class Functions
    {
        private readonly IWidgetService widgetService;

        public Functions(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }

        public async Task TestWebJob([QueueTrigger("webjobtestqueue")] string message, TextWriter log)
        {
            try
            {
                var widgets = await widgetService.GetWidgets(100);
                await log.WriteLineAsync("Widgets Returned");
            }
            catch (Exception ex)
            {
                await log.WriteLineAsync("error: " + ex.ToString());
            }
        }
    }
}
