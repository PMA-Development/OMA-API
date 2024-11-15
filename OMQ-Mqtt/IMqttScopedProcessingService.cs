using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMQ_Mqtt
{
    public interface IMqttScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
