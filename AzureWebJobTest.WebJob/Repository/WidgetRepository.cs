using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
            var sql = @"
declare @count int = 100000;
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
            await db.OpenAsync();
            return (List<Models.Widget>)await db.QueryAsync<Models.Widget>(sql);
        }
    }
}
