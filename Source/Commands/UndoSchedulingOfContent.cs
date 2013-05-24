namespace NServiceBusDemo.Commands
{
    using NServiceBus;

    public class UndoSchedulingOfContent : ICommand
    {
        public string PostScheduleId { get; set; }
    }
}
