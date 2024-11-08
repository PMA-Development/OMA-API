using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class TaskExtensions
    {
        public static IEnumerable<TaskDTO> ToDTOs(this IQueryable<OMA_Data.Entities.Task> source)
        {
            List<OMA_Data.Entities.Task> items = source.ToList();
            List<TaskDTO> DTOs = [];
            foreach (OMA_Data.Entities.Task item in items)
            {
                DTOs.Add(new TaskDTO
                {
                    TaskID = item.TaskID,
                    Description = item.Description,
                    FinishDescription = item.FinishDescription,
                    Owner = item.Owner,
                    Title = item.Title,
                    Turbine = item.Turbine,
                    Type = item.Type,
                    User = item.User
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<TaskDTO> ToDTOs(this IEnumerable<OMA_Data.Entities.Task> source)
        {
            List<TaskDTO> DTOs = [];
            foreach (OMA_Data.Entities.Task item in source)
            {
                DTOs.Add(new TaskDTO
                {
                    TaskID = item.TaskID,
                    Description = item.Description,
                    FinishDescription = item.FinishDescription,
                    Owner = item.Owner,
                    Title = item.Title,
                    Turbine = item.Turbine,
                    Type = item.Type,
                    User = item.User
                });
            }
            return DTOs;
        }

        public static OMA_Data.Entities.Task FromDTO(this TaskDTO source)
        {
            OMA_Data.Entities.Task item = new()
            {
                TaskID = source.TaskID,
                Description = source.Description,
                FinishDescription = source.FinishDescription,
                Owner = source.Owner,
                Title = source.Title,
                Turbine = source.Turbine,
                Type = source.Type,
                User = source.User
            };

            return item;
        }
    }
}
