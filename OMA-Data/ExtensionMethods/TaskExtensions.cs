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
    public static class TaskExtensions
    {
        #region InitializeRepo
        private static IGenericRepository<User> _genericUser;
        private static IGenericRepository<Turbine> _genericTurbine;
        public static IGenericRepository<User> GenericUser
        {
            get { return _genericUser; }
        }
        public static IGenericRepository<Turbine> GenericTurbine
        {
            get { return _genericTurbine; }
        }

        public static void InitRepo(IGenericRepository<User> genericUser, IGenericRepository<Turbine> genericTurbine)
        {
            _genericUser = genericUser;
            _genericTurbine = genericTurbine;
        }
        #endregion

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
                    OwnerID = item.Owner.UserID,
                    Title = item.Title,
                    TurbineID = item.Turbine.TurbineID,
                    Type = item.Type,
                    UserID = item.User.UserID
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
                    OwnerID = item.Owner.UserID,
                    Title = item.Title,
                    TurbineID = item.Turbine.TurbineID,
                    Type = item.Type,
                    UserID = item.User.UserID
                });
            }
            return DTOs;
        }

        public static async Task<OMA_Data.Entities.Task> FromDTO(
            this TaskDTO source,
            IGenericRepository<User> userRepository,
            IGenericRepository<Turbine> turbineRepository)
        {
            OMA_Data.Entities.Task item = new()
            {
                TaskID = source.TaskID,
                Description = source.Description,
                FinishDescription = source.FinishDescription,
                Type = source.Type,
                Title = source.Title,
                Owner = await userRepository.GetByIdAsync(source.OwnerID),
                Turbine = await turbineRepository.GetByIdAsync(source.TurbineID),
                User = source.UserID != null ? await userRepository.GetByIdAsync(source.UserID.Value) : null
            };

            return item;
        }
        public static TaskDTO ToDTO(this Entities.Task source)
        {
            TaskDTO item = new()
            {
                TaskID = source.TaskID,
                Description = source.Description,
                FinishDescription = source.FinishDescription,
                OwnerID = source.Owner.UserID,
                Title = source.Title,
                TurbineID = source.Turbine.TurbineID,
                Type = source.Type,
                UserID = source.User?.UserID,
            };

            return item;
        }
    }
}
