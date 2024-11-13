using OMA_Data.Core.Utils;
using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class LogExtensions
    {
        public static IEnumerable<LogDTO>? ToDTOs(this IQueryable<Log> source)
        {
            if (source == null)
                return default;

            List<Log> items = source.ToList();
            List<LogDTO> DTOs = [];
            foreach (Log item in items)
            {
                DTOs.Add(new LogDTO
                {
                    LogID = item.LogID,
                    Severity = item.Severity,
                    Description = item.Description,
                    Time = item.Time,
                    UserID = item.User.UserID,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<LogDTO>? ToDTOs(this IEnumerable<Log> source)
        {
            if (source == null)
                return default;

            List<LogDTO> DTOs = [];
            foreach (Log item in source)
            {
                DTOs.Add(new LogDTO
                {
                    LogID = item.LogID,
                    Severity = item.Severity,
                    Description = item.Description,
                    Time = item.Time,
                    UserID = item.User.UserID,
                });
            }
            return DTOs;
        }

        public static async Task<Log?> FromDTO(this LogDTO source, IGenericRepository<User> genericUser)
        {
            if (source == null)
                return default;

            Log item = new()
            {
                LogID = source.LogID,
                Severity = source.Severity,
                Description = source.Description,
                Time = source.Time,
                User = await genericUser.GetByIdAsync(source.UserID),
            };

            return item;
        }
        public static LogDTO? ToDTO(this Log source)
        {
            if (source == null)
                return default;

            LogDTO item = new()
            {
                LogID = source.LogID,
                Severity = source.Severity,
                Description = source.Description,
                Time = source.Time,
                UserID = source.User.UserID,
            };

            return item;
        }
    }
}
