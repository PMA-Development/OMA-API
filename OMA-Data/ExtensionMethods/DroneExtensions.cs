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
    public static class DroneExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<Entities.Task> _genericTask;
        public static IGenericRepository<Entities.Task> GenericTask
        {
            get { return _genericTask; }
        }
        public static void InitRepo(IGenericRepository<Entities.Task> genericTask)
        {
            _genericTask = genericTask;
        }
        #endregion

        public static IEnumerable<DroneDTO> ToDTOs(this IQueryable<Drone> source)
        {
            List<Drone> items = source.ToList();
            List<DroneDTO> DTOs = [];
            foreach (Drone item in items)
            {
                DTOs.Add(new DroneDTO
                {
                    DroneID = item.DroneID,
                    Available = item.Available,
                    TaskID = item.Task.TaskID,
                    Title = item.Title
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<DroneDTO> ToDTOs(this IEnumerable<Drone> source)
        {
            List<DroneDTO> DTOs = [];
            foreach (Drone item in source)
            {
                DTOs.Add(new DroneDTO
                {
                    DroneID = item.DroneID,
                    Available = item.Available,
                    TaskID = item.Task.TaskID,
                    Title = item.Title
                });
            }
            return DTOs;
        }

        public static async Task<Drone> FromDTO(this DroneDTO source)
        {
            Drone item = new()
            {
                DroneID = source.DroneID,
                Available = source.Available,
                Task = await _genericTask.GetByIdAsync(source.TaskID),
                Title = source.Title
            };

            return item;
        }
    }
}
