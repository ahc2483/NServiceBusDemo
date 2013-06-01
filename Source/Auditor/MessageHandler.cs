namespace NServiceBusDemo.Auditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NServiceBus;

    public class MessageHandler : IHandleMessages<Messages.PostScheduleCompleted>
    {
        public void Handle(Messages.PostScheduleCompleted message)
        {
            Console.WriteLine(string.Format("PostSchedule: {0} has completed!", message.PostScheduleId));
        }
    }
}
