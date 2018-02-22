using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureWebJobTest.ConsoleApp
{
    class Program
    {
        public static readonly string QUEUE_CONTAINER_NAME = "webjobtestqueue";

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                .Build();

            var queueConnectionString = config.GetConnectionString("AzureQueue");
            
            for (var i=0; i<15; i++)
            {
                using (var task = AddWebJobQueueMessage(queueConnectionString, QUEUE_CONTAINER_NAME, "console loop: " + i.ToString()))
                {
                    task.Wait();
                }
            }
            Console.WriteLine("Press Enter to Quit");
            Console.ReadLine();
        }

        private static async Task AddWebJobQueueMessage(string connectionString, string queueName, string message)
        {
            try
            {
                var azureAccount = CloudStorageAccount.Parse(connectionString);
                var queueClient = azureAccount.CreateCloudQueueClient();
                var queue = queueClient.GetQueueReference(queueName);
                await queue.CreateIfNotExistsAsync();
                var queueMessage = new CloudQueueMessage(message);
                await queue.AddMessageAsync(queueMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }
    }
}
