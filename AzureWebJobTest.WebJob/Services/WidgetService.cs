using AzureWebJobTest.WebJob.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureWebJobTest.WebJob.Services
{
    public interface IWidgetService
    {
        Task<List<Models.Widget>> GetWidgets(int count);
    }

    public class WidgetService : IWidgetService
    {
        private readonly IWidgetRepository widgetRepository;

        public WidgetService(IWidgetRepository widgetRepository)
        {
            this.widgetRepository = widgetRepository;
        }

        public async Task<List<Models.Widget>> GetWidgets(int count)
        {
            var allWidgets = await widgetRepository.GetWidgets();

            return allWidgets.Take(count).ToList();
        }
    }
}
