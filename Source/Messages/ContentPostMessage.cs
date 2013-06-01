using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusDemo.Messages
{
    public class ContentPostMessage : IMessage
    {
        public string PostId { get; set; }
        public string PageId { get; set; }
        public string PostScheduleId { get; set; }
    }
}
