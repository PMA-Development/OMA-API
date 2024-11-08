using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMA_Data.Core.Repositories;
using OMA_Data.Core.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Data
{
    public class DataContext(OMAContext context) : IDataContext
    {
        private readonly OMAContext _context = context;

        public IUserRepository UserRepository { get; private set; } = new UserRepository(context);
        public IAlarmConfigRepository AlarmConfigRepository { get; private set; } = new AlarmConfigRepository(context);
        public IAlarmRepository AlarmRepository { get; private set; } = new AlarmRepository(context);
        public IAttributeRepository AttributeRepository { get; private set; } = new AttributeRepository(context);
        public IDroneRepository DroneRepository { get; private set; } = new DroneRepository(context);
        public IIslandRepository IslandRepository { get; private set; } = new IslandRepository(context);
        public ILogRepository LogRepository { get; private set; } = new LogRepository(context);
        public ISensorRepository SensorRepository { get; private set; } = new SensorRepository(context);
        public ITaskRepository TaskRepository { get; private set; } = new TaskRepository(context);
        public ITurbineRepository TurbineRepository { get; private set; } = new TurbineRepository(context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public virtual EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class
        {
            return this._context.Entry(entity);
        }
    }
}
