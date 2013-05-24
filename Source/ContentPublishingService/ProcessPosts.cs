namespace NServiceBusDemo.ContentPublishingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NServiceBus.Saga;
    using NServiceBusDemo.Commands;

    public class ProcessPosts : Saga<ContentPostData>,
        IAmStartedByMessages<ProcessScheduledPosts>
    {
        public void Handle(ProcessScheduledPosts message)
        {
            throw new NotImplementedException();
        }
    }
}
