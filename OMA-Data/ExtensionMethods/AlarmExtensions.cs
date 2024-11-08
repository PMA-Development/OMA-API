using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class AlarmExtensions
    {
        public static IEnumerable<AlarmDTO> ToDTOs(this IQueryable<Alarm> source)
        {
            List<Alarm> items = source.ToList();
            List<AlarmDTO> DTOs = [];
            foreach (Alarm item in items)
            {
                DTOs.Add(new AlarmDTO
                {
                    AlarmID = item.AlarmID,
                    Island  = item.Island,
                    Turbine = item.Turbine
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<AlarmDTO> ToDTOs(this IEnumerable<Alarm> source)
        {
            List<AlarmDTO> DTOs = [];
            foreach (Alarm item in source)
            {
                DTOs.Add(new AlarmDTO
                {
                    AlarmID = item.AlarmID,
                    Island = item.Island,
                    Turbine = item.Turbine
                });
            }
            return DTOs;
        }

        public static Alarm FromDTO(this AlarmDTO source)
        {
            Alarm item = new()
            {
                AlarmID = source.AlarmID,
                Island = source.Island,
                Turbine = source.Turbine
            };

            return item;
        }
    }
}
