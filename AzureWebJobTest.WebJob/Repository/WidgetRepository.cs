using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureWebJobTest.WebJob.Repository
{
    public interface IWidgetRepository
    {
        Task<List<Models.Widget>> GetWidgets();
    }

    public class WidgetRepository : IWidgetRepository
    {
        private readonly WidgetDbContext dbContext;

        public WidgetRepository(WidgetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Models.Widget>> GetWidgets()
        {
            /**
             * The point of this sql is to make the server take 3-10 seconds to respond.
             * If you're running on a beefy sql data server you might need to bump up the count.
             */
            var sql = @"
declare @count int = 5000;
declare @index int = @count;
            declare @guid uniqueidentifier;
            create table #tmpWidgets ( widgetId bigint );

declare slow_cursor cursor for select NEWID()
open slow_cursor

while @index > 0
begin

    fetch next from slow_cursor into @guid

    insert into #tmpWidgets (widgetId) values (@index)

	set @index = @index - 1;

            end
            close slow_cursor;
            deallocate slow_cursor;

            select* from #tmpWidgets;

if (OBJECT_ID('tempdb..#tmpWidgets') is not null)
begin
    drop table #tmpWidgets
end";

            var db = dbContext.Database.GetDbConnection();
            if (db.State != System.Data.ConnectionState.Open)
            {
                await db.OpenAsync();
            }
            return (List<Models.Widget>)await db.QueryAsync<Models.Widget>(sql);
        }
    }
}
