using OMA_Data.Data;
using OMA_Data.Models;
using OMA_Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Services.Service
{
    public class UserService(OMAContext context) : IUserService
    {
        private readonly OMAContext _context = context;

        public User GetTask(int id)
        {
            if (_context == null)
            {
                return _context!.Users.Where(x => x.UserID == id).FirstOrDefault()!;
            }
            return null!;
        }
    }
}
