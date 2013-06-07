namespace NServiceBusDemo.Events
{
    using System.Collections.Generic;
    using NServiceBus;

    public interface PostScheduleCompleted : IEvent
    {
        string PostScheduleId { get; set; }
        IEnumerable<string> Successes { get; set; }
        IEnumerable<string> Failures { get; set; }
    }
}
