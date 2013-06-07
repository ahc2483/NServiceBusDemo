namespace NServiceBusDemo.Auditor
{
    using System;
    using NServiceBusDemo.Events;
    using NServiceBus;

    public class PostScheduledCompletedEventHandler : IHandleMessages<PostScheduleCompleted>
    {
        public void Handle(PostScheduleCompleted message)
        {
            Console.WriteLine(string.Format("PostSchedule: {0} has completed! Customer will now be billed $50.00", message.PostScheduleId));
        }
    }
}
