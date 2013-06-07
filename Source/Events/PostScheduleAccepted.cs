namespace NServiceBusDemo.Events
{
    using NServiceBus;

    public interface PostScheduleAccepted : IEvent
    {
        string PostScheduleId { get; set; }
    }
}
