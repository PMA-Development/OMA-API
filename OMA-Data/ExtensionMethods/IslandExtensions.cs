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
    public static class IslandExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<Turbine> _genericTurbine;
        public static IGenericRepository<Turbine> GenericTurbine
        {
            get { return _genericTurbine; }
        }
        public static void InitRepo(IGenericRepository<Turbine> genericTurbine)
        {
            _genericTurbine = genericTurbine;
        }
        #endregion
        public static IEnumerable<IslandDTO>? ToDTOs(this IQueryable<Island> source)
        {
            if (source == null)
                return default;

            List<Island> items = source.ToList();
            List<IslandDTO> DTOs = [];
            foreach (Island item in items)
            {
                DTOs.Add(new IslandDTO
                {
                    IslandID = item.IslandID,
                    Abbreviation = item.Abbreviation,
                    Title = item.Title,
                    ClientID = item.ClientID,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<IslandDTO>? ToDTOs(this IEnumerable<Island> source)
        {
            if (source == null)
                return default;

            List<IslandDTO> DTOs = [];
            foreach (Island item in source)
            {
                DTOs.Add(new IslandDTO
                {
                    IslandID = item.IslandID,
                    Abbreviation = item.Abbreviation,
                    Title = item.Title,
                    ClientID = item.ClientID
                });
            }
            return DTOs;
        }

        public static Island? FromDTO(this IslandDTO source)
        {
            if (source == null)
                return default;

            Island item = new()
            {
                IslandID = source.IslandID,
                Abbreviation = source.Abbreviation,
                Title = source.Title,
                ClientID = source.ClientID,
                Turbines = _genericTurbine.GetAll().Where(x => x.Island.IslandID == source.IslandID).ToList(),
            };

            return item;
        }
        public static IslandDTO? ToDTO(this Island source)
        {
            if (source == null)
                return default;

            IslandDTO item = new()
            {
                IslandID = source.IslandID,
                Abbreviation = source.Abbreviation,
                Title = source.Title,
                ClientID = source.ClientID,
            };

            return item;
        }
    }
}
