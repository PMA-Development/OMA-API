using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class TurbineExtensions
    {
        public static IEnumerable<TurbineDTO> ToDTOs(this IQueryable<Turbine> source)
        {
            List<Turbine> items = source.ToList();
            List<TurbineDTO> DTOs = [];
            foreach (Turbine item in items)
            {
                DTOs.Add(new TurbineDTO
                {
                    TurbineID = item.TurbineID,
                    Title = item.Title,
                    Sensor = item.Sensor,

                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<TurbineDTO> ToDTOs(this IEnumerable<Turbine> source)
        {
            List<TurbineDTO> DTOs = [];
            foreach (Turbine item in source)
            {
                DTOs.Add(new TurbineDTO
                {
                    TurbineID = item.TurbineID,
                    Title = item.Title,
                    Sensor = item.Sensor,
                });
            }
            return DTOs;
        }

        public static Turbine FromDTO(this TurbineDTO source)
        {
            Turbine item = new()
            {
                TurbineID = source.TurbineID,
                Title = source.Title,
                Sensor = source.Sensor,
            };

            return item;
        }
    }
}
