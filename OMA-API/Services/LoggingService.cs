using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using OMA_API.Services.Interfaces;
using OMA_Data.Data;
using OMA_Data.Entities;

namespace OMA_API.Services
{
    public class LoggingService(IDataContext context, IHttpContextAccessor httpContext) : ILoggingService
    {
        private readonly IDataContext _context = context;
        private readonly IHttpContextAccessor _httpContext = httpContext;

        public async System.Threading.Tasks.Task AddLog(LogLevel logLevel, string description) 
        {
            //TODO : Implent the below code when done testing.
            //var userID = _httpContext.HttpContext!.User.Identity.GetUserId();
            User user = await _context.UserRepository.GetByIdAsync(Guid.Parse("c6936336-4a10-4445-b373-60f6a37a58c4"));

            await _context.LogRepository.Add(new Log() { User = user, Severity = logLevel.ToString(), Description = description});

            await _context.CommitAsync();

        }
    }
}
