namespace NServiceBusDemo.Auditor
{
    using System;
    using NServiceBus;

    public class PostScheduledCompletedEventHandler : IHandleMessages<Messages.PostScheduleCompleted>
    {
        public void Handle(Messages.PostScheduleCompleted message)
        {
            Console.WriteLine(string.Format("PostSchedule: {0} has completed! Customer will now be billed $50.00", message.PostScheduleId));
        }
    }
}
