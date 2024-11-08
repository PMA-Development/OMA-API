using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class IslandExtensions
    {
        public static IEnumerable<IslandDTO> ToDTOs(this IQueryable<Island> source)
        {
            List<Island> items = source.ToList();
            List<IslandDTO> DTOs = [];
            foreach (Island item in items)
            {
                DTOs.Add(new IslandDTO
                {
                    IslandID = item.IslandID,
                    Abbreviation = item.Abbreviation,
                    Title = item.Title,
                    Turbine = item.Turbine
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<IslandDTO> ToDTOs(this IEnumerable<Island> source)
        {
            List<IslandDTO> DTOs = [];
            foreach (Island item in source)
            {
                DTOs.Add(new IslandDTO
                {
                    IslandID = item.IslandID,
                    Abbreviation = item.Abbreviation,
                    Title = item.Title,
                    Turbine = item.Turbine
                });
            }
            return DTOs;
        }

        public static Island FromDTO(this IslandDTO source)
        {
            Island item = new()
            {
                IslandID = source.IslandID,
                Abbreviation = source.Abbreviation,
                Title = source.Title,
                Turbine = source.Turbine
            };

            return item;
        }
    }
}
