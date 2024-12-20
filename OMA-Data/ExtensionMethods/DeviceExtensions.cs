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
    public static class DeviceExtensions
    {
        public static IEnumerable<DeviceDTO>? ToDTOs(this IQueryable<Device> source)
        {
            if (source == null)
                return default;

            List<Device> items = source.ToList();
            List<DeviceDTO> DTOs = [];
            foreach (Device item in items)
            {
                DTOs.Add(new DeviceDTO
                {
                    DeviceId = item.DeviceId,
                    State = item.State,
                    Type = item.Type,
                    ClientID = item.ClientID,
                    TurbineID = item.Turbine.TurbineID,
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<DeviceDTO>? ToDTOs(this IEnumerable<Device> source)
        {
            if (source == null)
                return default;

            List<DeviceDTO> DTOs = [];
            foreach (Device item in source)
            {
                DTOs.Add(new DeviceDTO
                {
                    DeviceId = item.DeviceId,
                    State = item.State,
                    Type = item.Type,
                    ClientID = item.ClientID,
                    
                    TurbineID = item.Turbine.TurbineID
                });
            }
            return DTOs;
        }

        public static async Task<Device?> FromDTO(this DeviceDTO source, IGenericRepository<Turbine> genericTurbine)
        {
            if (source == null)
                return default;

            Device item = new()
            {
                DeviceId = source.DeviceId,
                State = source.State,
                Type = source.Type,
                ClientID = source.ClientID,
                Turbine = await genericTurbine.GetByIdAsync(source.TurbineID)
            };
            return item;
        }
        
        public static DeviceDTO? ToDTO(this Device source)
        {
            if (source == null)
                return default;

            DeviceDTO item = new()
            {
                DeviceId = source.DeviceId,
                State = source.State,
                Type = source.Type,
                ClientID = source.ClientID,
                TurbineID = source.Turbine.TurbineID,
            };
            return item;
        }
    }
}
