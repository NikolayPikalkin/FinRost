using FinRost.BL.Dto.Web.Log;
using FinRost.DAL;
using FinRost.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinRost.BL.Services
{
    public class LogService
    {
        private readonly ApplicationDbContext _db;
        private readonly IServiceProvider _serviceProvider;
        public LogService(IServiceProvider serviceProvider, ApplicationDbContext db)
        {
            _serviceProvider = serviceProvider;
            _db = db;
        }

        public async Task AddError(string logMessage)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var logError = new ServiceLog
            {
                CreationDate = DateTime.Now,
                EntityId = 0,
                Title = "Back",
                LogMessage = logMessage
            };

            dbContext.ServiceLogs.Add(logError);
            await dbContext.SaveChangesAsync();
        }

        public async Task<LogResponse> GetLogsAsync(int entityId, int page) // entityId - 0 - back 1 - front
        {
            int pageSize = 30;

            var logs = await _db.ServiceLogs.Where(it => it.EntityId == entityId)
                                            .OrderByDescending(it => it.CreationDate)
                                            .Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

            var totalPages = ((await _db.ServiceLogs.CountAsync(it => it.EntityId == entityId))/pageSize) + 1;


            return new LogResponse { Logs = logs, TotalPages = totalPages };
        } 

        public async Task<ServiceLog> AddFrontLogAsync(LogRequest logInfo)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var log = new ServiceLog
            {
                CreationDate = DateTime.Now,
                EntityId = 1,
                Title = "Front: " + logInfo.Title,
                LogMessage = logInfo.Message,
            };

            dbContext.ServiceLogs.Add(log);
            await dbContext.SaveChangesAsync();

            return log;
        }
    }
}
