using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusDemo.Messages
{
    public class PostScheduleCompleted : IEvent
    {
        public string PostScheduleId { get; set; }
        public IEnumerable<string> Successes { get; set; }
        public IEnumerable<string> Failures { get; set; }
    }
}
