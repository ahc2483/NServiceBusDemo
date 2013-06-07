namespace NServiceBusDemo.Auditor
{
    using System;
    using NServiceBus;
    using NServiceBusDemo.Events;

    public class PostScheduleAmplificationCompletedHandler : IHandleMessages<PostScheduleAmplificationCompleted>
    {
        public void Handle(PostScheduleAmplificationCompleted message)
        {
            Console.WriteLine(string.Format("PostScheduleAmplification: {0} has completed! Customer will now be billed $150.00", message.PostScheduleId));
        }
    }
}
