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
    public class TurbineRepository(OMAContext context) : GenericRepository<Turbine>(context), ITurbineRepository
    {
        public virtual List<Turbine> GetTurbinesByIslandId(int id)
        {
            return _context.Turbines.Where(x => x.Island.IslandID == id).ToList();
        }

        public virtual Turbine? GetByClintId(string id)
        {
            return _dbContext.Where(s => s.ClientID == id).FirstOrDefault();
        }
    }
}
