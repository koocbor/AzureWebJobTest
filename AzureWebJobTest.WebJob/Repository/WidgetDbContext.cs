using Microsoft.EntityFrameworkCore;

namespace AzureWebJobTest.WebJob.Repository
{
    public class WidgetDbContext : DbContext
    {
        public WidgetDbContext(DbContextOptions<WidgetDbContext> options) : base(options) { }
    }
}
