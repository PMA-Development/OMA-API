using OMA_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Services.Interface
{
    public interface IUserService
    {
        User GetTask(int id);
    }
}
