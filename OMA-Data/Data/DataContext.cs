using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMA_Data.Core.Repositories;
using OMA_Data.Core.Repositories.Interface;

namespace OMA_Data.Data
{
    public class DataContext(OMAContext context) : IDataContext
    {
        private readonly OMAContext _context = context;

        public IUserRepository UserRepository { get; private set; } = new UserRepository(context);
        public IAlarmConfigRepository AlarmConfigRepository { get; private set; } = new AlarmConfigRepository(context);
        public IAlarmRepository AlarmRepository { get; private set; } = new AlarmRepository(context);
        public IDroneRepository DroneRepository { get; private set; } = new DroneRepository(context);
        public IIslandRepository IslandRepository { get; private set; } = new IslandRepository(context);
        public ILogRepository LogRepository { get; private set; } = new LogRepository(context);
        public IDeviceRepository DeviceRepository { get; private set; } = new DeviceRepository(context);
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
