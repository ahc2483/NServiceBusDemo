namespace NServiceBusDemo.Commands
{
    using System.Collections.Generic;
    using NServiceBus;

    public class ProcessScheduledPosts : ICommand
    {
        public Dictionary<string, string> PagePosts { get; set; }
    }
}
