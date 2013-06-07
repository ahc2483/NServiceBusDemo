namespace NServiceBusDemo.Events
{
    using NServiceBus;

    public interface PostScheduleAmplificationCompleted : IEvent
    {
        string PostScheduleId { get; set; }
    }
}
