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
                    Task = item.Task,
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
                    Task = item.Task,
                    Title = item.Title
                });
            }
            return DTOs;
        }

        public static Drone FromDTO(this DroneDTO source)
        {
            Drone item = new()
            {
                DroneID = source.DroneID,
                Available = source.Available,
                Task = source.Task,
                Title = source.Title
            };

            return item;
        }
    }
}
