using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Mqtt.Models
{
    // Parsed object to the MQTT network
    public class ChangeState
    {
        public required string Value { get; set; }
    }
}
