using OMA_Data.DTOs;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.ExtensionMethods
{
    public static class UserExtensions
    {
        public static IEnumerable<UserDTO> ToDTOs(this IQueryable<User> source)
        {
            List<User> items = source.ToList();
            List<UserDTO> DTOs = [];
            foreach (User item in items)
            {
                DTOs.Add(new UserDTO
                {
                    UserID = item.UserID,
                    Name = item.Name
                });
            }
            return DTOs;
        }

        //used by LINQ to Linq
        public static IEnumerable<UserDTO> ToDTOs(this IEnumerable<User> source)
        {
            List<UserDTO> DTOs = [];
            foreach (User item in source)
            {
                DTOs.Add(new UserDTO
                {
                    UserID = item.UserID,
                    Name = item.Name
                });
            }
            return DTOs;
        }

        public static User FromDTO(this UserDTO source)
        {
            User item = new()
            {
                UserID = source.UserID,
                Name = source.Name
            };

            return item;
        }
    }
}
