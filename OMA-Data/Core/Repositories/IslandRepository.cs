using OMA_Data.Core.Repositories.Interface;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Core.Repositories
{
    public class IslandRepository(OMAContext context) : GenericRepository<Island>(context), IIslandRepository
    {
    }
}
