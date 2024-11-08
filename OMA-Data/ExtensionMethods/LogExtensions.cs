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
        public static IEnumerable<LogDTO> ToDTOs(this IQueryable<Log> source)
        {
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
                    User = item.User,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<LogDTO> ToDTOs(this IEnumerable<Log> source)
        {
            List<LogDTO> DTOs = [];
            foreach (Log item in source)
            {
                DTOs.Add(new LogDTO
                {
                    LogID = item.LogID,
                    Severity = item.Severity,
                    Description = item.Description,
                    Time = item.Time,
                    User = item.User,
                });
            }
            return DTOs;
        }

        public static Log FromDTO(this LogDTO source)
        {
            Log item = new()
            {
                LogID = source.LogID,
                Severity = source.Severity,
                Description = source.Description,
                Time = source.Time,
                User = source.User,
            };

            return item;
        }
    }
}
