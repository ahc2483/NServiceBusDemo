namespace NServiceBusDemo.Commands
{
    using System.Collections.Generic;
    using NServiceBus;

    public class ScheduleContentPosts : ICommand
    {
        public string PostScheduleId { get; set; }
        public Dictionary<string, string> PagePosts { get; set; }
    }
}
