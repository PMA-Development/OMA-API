using Microsoft.EntityFrameworkCore;
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
        public virtual Island? GetByClintId(string id)
        {
            return _dbContext.Where(s => s.ClientID == id).FirstOrDefault();
        }

        protected new OMAContext _context = context;

        public List<Entities.Task> GetTaskForIsland(int id)
        {
            List<Entities.Task> listOfTaskForIsland = [];
            _context.Tasks.ToList().ForEach(tasks =>
            {
                _context.Turbines.Include(x => x.Island).Where(x => x.Island.IslandID == id).ToList().ForEach(turbine =>
                {
                    if (tasks.Turbine.TurbineID == turbine.TurbineID)
                        listOfTaskForIsland.Add(tasks);
                });
            });

            return listOfTaskForIsland;
        }
    }
}
