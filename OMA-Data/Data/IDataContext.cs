using Microsoft.EntityFrameworkCore.ChangeTracking;
using OMA_Data.Core.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Data.Data
{
    public interface IDataContext
    {
        IUserRepository UserRepository { get; }
        IAlarmConfigRepository AlarmConfigRepository { get; }
        IAlarmRepository AlarmRepository { get; }
        IAttributeRepository AttributeRepository { get; }
        IDroneRepository DroneRepository { get; }
        IIslandRepository IslandRepository { get; }
        ILogRepository LogRepository { get; }
        ISensorRepository SensorRepository { get; }
        ITaskRepository TaskRepository { get; }
        ITurbineRepository TurbineRepository { get; }

        Task CommitAsync();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
