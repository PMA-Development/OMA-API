using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMQ_Mqtt.Models
{
    // Parsed object to the MQTT network
    public class ChangeState
    {
        public required int Value { get; set; }
    }
}
