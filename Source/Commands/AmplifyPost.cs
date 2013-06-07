namespace NServiceBusDemo.Commands
{
    using NServiceBus;

    public class AmplifyPost : ICommand
    {
        public string PostId { get; set; }
    }
}
