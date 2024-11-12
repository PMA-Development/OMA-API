﻿using OMA_Data.Core.Utils;
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
        #region InitializeRepo
        private static IGenericRepository<Island> _genericIsland;
        private static IGenericRepository<Device> _genericDevice;
        public static IGenericRepository<Island> GenericIsland
        {
            get { return _genericIsland; }
        }
        public static IGenericRepository<Device> GenericDevice
        {
            get { return _genericDevice; }
        }
        public static void InitRepo(IGenericRepository<Island> genericIsland, IGenericRepository<Device> genericDevice)
        {
            _genericIsland = genericIsland;
            _genericDevice = genericDevice;
        }
        #endregion
        public static IEnumerable<TurbineDTO>? ToDTOs(this IQueryable<Turbine> source)
        {
            if (source == null)
                return default;

            List<Turbine> items = source.ToList();
            List<TurbineDTO> DTOs = [];
            foreach (Turbine item in items)
            {
                foreach (Device device in item.Devices)
                {
                    DTOs.Add(new TurbineDTO
                    {
                        TurbineID = item.TurbineID,
                        Title = item.Title,
                        IslandID = item.Island.IslandID,
                        ClientID = device.ClientID,
                    });

                }
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<TurbineDTO>? ToDTOs(this IEnumerable<Turbine> source)
        {
            if (source == null)
                return default;

            List<TurbineDTO> DTOs = [];
            foreach (Turbine item in source)
            {
                foreach (Device device in item.Devices)
                {
                    DTOs.Add(new TurbineDTO
                    {
                        TurbineID = item.TurbineID,
                        Title = item.Title,
                        IslandID = item.Island.IslandID,
                        ClientID = item.ClientID,
                    });
                }
            }
            return DTOs;
        }

        public static async Task<Turbine?> FromDTO(this TurbineDTO source)
        {
            if (source == null)
                return default;

            Turbine item = new()
            {
                TurbineID = source.TurbineID,
                Title = source.Title,
                Island = await _genericIsland.GetByIdAsync(source.IslandID),
                Devices = _genericDevice.GetAll().Where(x => x.Turbine.TurbineID == source.TurbineID).ToList(),
                ClientID = source.ClientID,
            };

            return item;
        }
        public static TurbineDTO? ToDTO(this Turbine source)
        {
            if (source == null)
                return default;

            TurbineDTO item = new()
            {
                TurbineID = source.TurbineID,
                Title = source.Title,
                IslandID = source.Island.IslandID,
                ClientID = source.ClientID,
            };

            return item;
        }
    }
}
