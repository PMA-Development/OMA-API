using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_Mqtt.Models
{
    // Enqueue this object to the change state request queue.
    public class ActionRequest
    {
        public required string ClientId { get; set; }
        public required string Action { get; set; }
        public required int Value { get; set; }
    }
}
